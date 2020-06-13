using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class TextPCopy
    {
        public const string spec = @"create or replace package pcopy is

  cLogLevelTrace constant number := 1;
  cLogLevelDebug constant number := 2;
  cLogLevelInfo  constant number := 3;
  cLogLevelWarn  constant number := 4;
  cLogLevelError constant number := 5;

  vLogLevel number := cLogLevelTrace;
  vDbLink varchar2(30) := 'tester.world';
  vSkipError boolean := false;

  type TCopyArgs is record(
    Isn             number,
    TableName       varchar2(30),
    OwnerName       varchar2(30) default 'AIS',
    DoUpdate        boolean default false,
    DoDelete        boolean default false, 
    GroupName       varchar2(40) 
  );
  
  procedure TriggersEnable(pTableName varchar2, pSchemaName varchar2 default 'AIS');
  procedure TriggersDisable(pTableName varchar2, pSchemaName varchar2 default 'AIS');
  
  procedure CopyObj(pCopyArgs TCopyArgs);

end pcopy;
/
";

        public const string body = @"create or replace package body pcopy is
  
  vProcessID number;
  vCopyArgs tCopyArgs;

  cDictiConst      constant varchar2(20) := 'PCOPY';
  cDicxConst       constant varchar2(20) := 'X' || cDictiConst;
  cDictiGroupConst constant varchar2(30) := cDictiConst || 'GROUP';
  
  type tFieldsList is table of varchar2(2000) index by varchar2(30);
  vFieldsList tFieldsList;
  
  type recTrigger is record (SchemaName varchar2(30), TableName varchar2(30), TriggerName varchar2(30)); 
  type tbTriggerList is table of recTrigger;  
  type tRowInfo is record(Isn      number,
                          TableIsn number,
                          OwnerIsn number);
                          
  /************************************************/                            
  /************** НАСТРОЙКИ ***********************/                          
  /************************************************/  
  
  procedure Msg(pMsg varchar2, pLevel number default cLogLevelInfo, pRow tRowInfo default null) is
  vMsg varchar2(2000) := pMsg;
  begin
    if pLevel < vLogLevel then
      return;
    end if;
    if pRow.TableIsn is not null then
      vMsg := vMsg || ' pRow.Owner: ' || pRow.OwnerIsn || ' [' || locdictname(pRow.OwnerIsn) || '], '
                   || ' pRow.Table: ' || pRow.TableIsn || ' [' || locdictname(pRow.TableIsn) || '], '
                   || ' pRow.Isn: ' || pRow.Isn;
    end if;
    ais.msg(vMsg);
  end;


  
  function GetObjIsn(pObjName varchar2, pParentIsn number default null)
    return number is
    vIsn       number;
    vParentIsn number := pParentIsn;
  begin
    if vParentIsn is null then
      vParentIsn := c.get(cDictiConst);
    end if;
    select isn into vIsn from dicti
     where parentisn = vParentIsn
       and shortname = upper(pObjName);
    return vIsn;
  exception
    when no_data_found then
      insert into dicti
        (parentisn, shortname)
      values
        (vParentIsn, upper(pObjName))
      returning isn into vIsn;
      return vIsn;
  end;

  function ToLinkRow(pBaseOwnerName varchar2,
                     pBaseTableName varchar2,
                     pBaseFieldName varchar2,
                     pLinkOwnerName varchar2,
                     pLinkTableName varchar2,
                     pLinkFieldName varchar2,
                     pLinkType      char) return pcopy_links%rowtype is
    vLinkRow pcopy_links%rowtype;
  begin
    vLinkRow.BaseOwnerIsn := GetObjIsn(pBaseOwnerName);
    vLinkRow.BaseTableIsn := GetObjIsn(pBaseTableName);
    vLinkRow.BaseFieldIsn := GetObjIsn(pBaseFieldName);
    vLinkRow.LinkOwnerIsn := GetObjIsn(pLinkOwnerName);
    vLinkRow.LinkTableIsn := GetObjIsn(pLinkTableName);
    vLinkRow.LinkFieldIsn := GetObjIsn(pLinkFieldName);
    vLinkRow.Linktype     := pLinkType;
    return vLinkRow;
  end;    

  function CreateLink(pLinkRow pcopy_links%rowtype) return number is
    vIsn number;
  begin
    insert into ais.pcopy_links
      (baseownerisn,
       basetableisn,
       basefieldisn,
       linkownerisn,
       linktableisn,
       linkfieldisn,
       linktype)
    values
      (pLinkRow.BaseOwnerIsn,
       pLinkRow.BaseTableIsn,
       pLinkRow.BaseFieldIsn,
       pLinkRow.LinkOwnerIsn,
       pLinkRow.LinkTableIsn,
       pLinkRow.LinkFieldIsn,
       pLinkRow.LinkType) return isn into vIsn;
     return vIsn;
  exception 
    when DUP_VAL_ON_INDEX then
      raise;       
  end;
  
  function GetLinkIsn(pLinkRow pcopy_links%rowtype) return number is
    vIsn number;
  begin
    begin
      select isn into vIsn
        from pcopy_links
       where baseownerisn = pLinkRow.BaseOwnerIsn
         and basetableisn = pLinkRow.BaseTableIsn
         and basefieldisn = pLinkRow.BaseFieldIsn
         and linkownerisn = pLinkRow.LinkOwnerIsn
         and linktableisn = pLinkRow.LinkTableIsn
         and linkfieldisn = pLinkRow.LinkFieldIsn
         and linktype = pLinkRow.LinkType;
    exception
      when no_data_found then
        vIsn := CreateLink(pLinkRow);
    end;
    return vIsn;
  end;

 
  procedure AddLinkToGroup(pGroupName varchar2,
                           pLinkRow   pcopy_links%rowtype) is
    vLinkIsn  number;
    vGroupIsn number;
    vCnt      number;
  begin
    vLinkIsn  := GetLinkIsn(pLinkRow);
    vGroupIsn := GetObjIsn(pGroupName, c.get(cDictiGroupConst));
  
    select count(1)
      into vCnt
      from dicx
     where classisn = c.get(cDicxConst)
       and classisn1 = vGroupIsn
       and classisn2 = vLinkIsn;
    if vCnt = 0 then
      insert into dicx
        (classisn, classisn1, classisn2)
      values
        (c.get(cDicxConst), vGroupIsn, vLinkIsn);
    end if;
  end;  
  
  procedure InitLinks is
  begin
    delete from pcopy_links;  
    delete from dicx where classisn = c.get(cDicxConst);
    
    AddLinkToGroup('agreement', ToLinkRow('ais', 'agreement', 'isn', 'ais', 'agrobject', 'agrisn', 'x'));
    AddLinkToGroup('agreement', ToLinkRow('ais', 'agreement', 'isn', 'ais', 'agrcond', 'agrisn', 'x'));
    AddLinkToGroup('agreement', ToLinkRow('ais', 'agreement', 'isn', 'ais', 'agrtariff', 'agrisn', 'x'));
    AddLinkToGroup('agreement', ToLinkRow('ais', 'agreement', 'isn', 'ais', 'agrext', 'agrisn', 'x'));
    
    commit;
  end;  

  procedure InitDictionary is
    vCnt number;
  begin
    select count(1) into vCnt from dicti where constname = cDictiConst;
    if vCnt = 0 then
      msg('Начинаем иницилизацию словарей');
      insert into dicti
        (parentisn, shortname, fullname, constname, active, n_children)
      values
        (114216,
         'Словарь для репликации объектов',
         'Словарь для репликации объектов',
         cDictiConst,
         'N',
         0);
    
      insert into dicti
        (parentisn, shortname, fullname, constname, active, n_children)
      values
        (114216,
         'Словарь для групп репликации',
         'Словарь для групп репликации',
         cDictiGroupConst,
         'N',
         0);
    
      insert into dicti
        (parentisn, shortname, fullname, constname, active)
      values
        (c.get('dicx'),
         'Группы репликации',
         'Группы репликации',
         cDicxConst,
         'N');
         
      InitLinks;
    end if;
  end InitDictionary;

  /************************************************/                            
  /************** РАБОТА С ТРИГГЕРАМИ *************/                          
  /************************************************/    
 
  -- Выбрать триггеры которые необходимо отключить
  function tr_GetTriggers(pTableName varchar2, pSchemaName varchar2)
    return tbTriggerList is
    vAns tbTriggerList := tbTriggerList();
  begin
    select table_owner, table_name, trigger_name
      bulk collect
      into vAns
      from all_triggers
     where table_owner = upper(pSchemaName)
       and table_name = upper(pTableName)
       and status = 'ENABLED'
       and regexp_like(trigger_name, '[^S]$');
    return vAns;
  end;
  
  -- Включаем/отключаем список тригеров
  procedure tr_TriggersEnabler(ptbTriggerList tbTriggerList, pEnable boolean) is
    vQuery varchar2(100);
    vEnable varchar2(10) := 'enable';
  begin
    if not pEnable then
      vEnable := 'disable';
    end if;
    if ptbTriggerList.count = 0 then
      return;
    end if;
    for i in ptbTriggerList.first .. ptbTriggerList.last
      loop
        vQuery := 'alter trigger ' || ptbTriggerList(i).SchemaName || '.' || ptbTriggerList(i).TriggerName || ' ' || vEnable;
        execute immediate vQuery;
      end loop;
  end;
  
  -- Сохраняем список отключенных тригеров в таблицу, что бы была возможность их включить
  procedure tr_SaveTrigerList(ptbTriggerList tbTriggerList) is
    PRAGMA AUTONOMOUS_TRANSACTION;
  begin
    forall i in 1 .. ptbTriggerList.count SAVE EXCEPTIONS
      insert into triggerstatus
        (schemaname, tablename, triggername, status, dateoff)
      values 
        (ptbTriggerList(i).SchemaName,
         ptbTriggerList(i).tablename,
         ptbTriggerList(i).triggername,
         0,
         systimestamp);
     commit;
  exception 
    when others then
    FOR indx IN 1 .. SQL%BULK_EXCEPTIONS.COUNT
    loop
      ais.msg('Error: Не удалось сохранить триггер ' || SQL%BULK_EXCEPTIONS(indx).ERROR_INDEX);
    end loop;  
    raise;
  end;
  
  -- Загружаем отключенные тригеры из сохранненой таблицы
  function tr_LoadTrigerList(pTableName varchar2, pSchemaName varchar2)
    return tbTriggerList is
    vRes tbTriggerList;
  begin
    select SchemaName, tablename, triggername
      bulk collect
      into vRes
      from triggerstatus
     where status = 0
       and SchemaName = upper(pSchemaName)
       and tablename = upper(pTableName);
    return vRes;
  end;
  
  -- Загружаем отключенные тригеры из сохранненой таблицы
  function tr_LoadAllTrigerList return tbTriggerList is
    vRes tbTriggerList;
  begin
    select SchemaName, tablename, triggername
      bulk collect
      into vRes
      from triggerstatus
     where status = 0;
    return vRes;
  end;  
  
  -- Устанавливаем в таблице статус для отключенных, что они были включены
  procedure tr_SetTrigerStatusOk(pTableName varchar2, pSchemaName varchar2)
    is
  PRAGMA AUTONOMOUS_TRANSACTION;
  begin
    update triggerstatus
       set status = 1, dateon = systimestamp
     where status = 0
       and SchemaName = upper(pSchemaName)
       and tablename = upper(pTableName);
    commit;
  end; 
  
  -- Устанавливаем в таблице статус для отключенных, что они были включены
  procedure tr_SetAllTrigerStatusOk
    is
  PRAGMA AUTONOMOUS_TRANSACTION;
  begin
    update triggerstatus
       set status = 1, dateon = systimestamp
     where status = 0;
    commit;
  end;     
  
  -- Включаем ранее выключенные тригерры для таблицы
  procedure TriggersEnable(pTableName varchar2, pSchemaName varchar2 default 'AIS')
  is
    vTriggers tbTriggerList;
  begin
    vTriggers := tr_LoadTrigerList(pTableName, pSchemaName);
    tr_TriggersEnabler(vTriggers, true);
    tr_SetTrigerStatusOk(pTableName, pSchemaName);
  end;
  
  procedure TriggersEnable is
    vTriggers tbTriggerList;
  begin
    vTriggers := tr_LoadAllTrigerList;
    tr_TriggersEnabler(vTriggers, true);
    tr_SetAllTrigerStatusOk;
  end;    
  
  -- Выключаем все включенные тригеры для таблицы
  procedure TriggersDisable(pTableName varchar2, pSchemaName varchar2 default 'AIS')
  is
    vTriggers tbTriggerList;
  begin
    vTriggers := tr_GetTriggers(pTableName, pSchemaName);
    tr_TriggersEnabler(vTriggers, false);
    tr_SaveTrigerList(vTriggers);
  end;
  
  procedure TriggersDisable(pRow tRowInfo) is
  begin
    TriggersDisable(locdictname(prow.TableIsn), locdictname(prow.OwnerIsn));
  end;
  
  /************************************************/                            
  /************** КОПИРОВАНИЕ *********************/                          
  /************************************************/  
  
  function GetTableName(pRow tRowInfo, pUseDbLink boolean default false) return varchar2 is
    vTableName varchar2(100);
  begin
    vTableName := nvl(locdictname(pRow.OwnerIsn), 'AIS') || '.' || locdictname(pRow.TableIsn);
    if pUseDbLink then
      vTableName := vTableName || '@' || vDbLink;
    end if;
    return vTableName;
  end;
  -- Проверить запрос на допустимую логику
  Procedure CheckQuery(pQuery varchar2) is
    procedure RunError(pMsg varchar2) is
      begin
        msg('CheckQuery: '|| pMsg, cLogLevelError);
        msg('pQuery=' || pQuery, cLogLevelError);
        raise_application_error(-20001, pMsg);
      end;
  begin
    if regexp_like(pQuery, '(update|insert|delete)\s+((into|(\*\s+)?from)\s+)?\w+@', 'i') then
        RunError('Запрещено изменять объекты через DBLINK');
    elsif regexp_like(pQuery, 'delete\s+(\*\s+)?from dicti\W', 'i') then
        RunError('Запрещено удалять из DICTI');
    end if;
    msg('Execute immediate: ' || pQuery, cLogLevelTrace);
  end;

  procedure SafeExecute(pQuery varchar2) is
  begin
    CheckQuery(pQuery);
    execute immediate pQuery;
  end;
  
  procedure SafeExecute(pQuery varchar2, pNumberList tNum) is
  begin
    CheckQuery(pQuery);
    execute immediate pQuery using pNumberList;
  end;

  function SafeExecute(pQuery varchar2) return number is
    vIsn number;
  begin
    CheckQuery(pQuery);
    execute immediate pQuery into vIsn;
    return vIsn;
  end;  

  function IsIsnExist(pRow tRowInfo, pUseDbLink boolean default false)
    return boolean is
    vQuery varchar2(4000);
    vIsn   number;
  begin
    vQuery := 'select max(isn) into :a from ' || GetTableName(pRow, pUseDbLink) || ' where isn  = ' || pRow.Isn;
    vIsn   := SafeExecute(vQuery);
    return vIsn is not null;
  end;  
  
  function GetTableFields(pRow tRowInfo) return varchar2 is
    vFields varchar2(2000) := '';
    vErrMsg varchar2(200);
  begin
    if vFieldsList.exists(pRow.TableIsn) then
      return vFieldsList(pRow.TableIsn);
    else
      for item in (select * from all_tab_columns 
                    where owner = upper(locdictname(pRow.OwnerIsn)) 
                      and table_name = upper(locdictname(pRow.TableIsn)))
      loop
        if length(vFields) != 0 then
          vFields := vFields || ', ';
        end if;
        vFields := vFields || item.column_name;
      end loop;
      if vFields is null then
        vErrMsg:='Не удалось определить список полей для таблицы: ' || locdictname(pRow.OwnerIsn) || ' схема: '|| locdictname(pRow.TableIsn);
        msg(vErrMsg, cLogLevelError);
        raise_application_error(-20001, vErrMsg);
      end if;
      vFieldsList(pRow.TableIsn) := vFields;
      return vFields;
    end if;
  end;  
  
  procedure PerfomActionOnRows(pRow tRowInfo, pRowsIsns tNum, pOperation char) is
    vQuery varchar2(4000);
  begin
    case pOperation 
      when 'I' then
        vQuery := 'insert into '|| GetTableName(pRow) ||' select * from ' || GetTableName(pRow, true) || ' where isn member of :x';
        SafeExecute(vQuery, pRowsIsns);
      when 'D' then
        vQuery := 'delete from '|| GetTableName(pRow) ||' where isn member of :x';
        SafeExecute(vQuery, pRowsIsns);
      when 'I' then
        if pRowsIsns.count > 0 then
          for i in pRowsIsns.First .. pRowsIsns.Last
          loop
            vQuery := 
               'update '||GetTableName(pRow)||' '
            || 'set('||GetTableFields(pRow)||') = (select '||GetTableFields(pRow)||' from '|| GetTableName(pRow, true) ||' '
            ||                                     'where isn = '|| pRowsIsns(i) ||') '
            || 'where isn = '|| pRowsIsns(i);
            SafeExecute(vQuery);
          end loop;
        end if;
    end case;
  exception
    when others then
      msg('ERROR', cLogLevelError);
      msg(dbms_utility.format_error_stack, cLogLevelError);
      msg('Произошла ошибка при обработки строк', cLogLevelError, pRow);
      msg('vQuery:' || chr(10) || vQuery, cLogLevelError);
      raise;
  end;
  
  procedure ProcessRows(pProcessID number) is
    vListRows tNum;
    vRowInfo tRowInfo;
  begin
    -- Отключаем все тригеры
    for t in (select ownerisn, tableisn from pcopy_rows
               where processid = pProcessID
               group by ownerisn, tableisn) 
    loop
      vRowInfo.OwnerIsn := t.ownerisn;
      vRowInfo.TableIsn := t.tableisn;
      TriggersDisable(vRowInfo);
    end loop;

    -- Обрабатываем строки из pcopy_rows
    for t in (select ownerisn, tableisn, operation from pcopy_rows
               where processid = pProcessID
               group by ownerisn, tableisn, operation
               order by ownerisn, tableisn, operation) 
    loop
      vRowInfo.OwnerIsn := t.ownerisn;
      vRowInfo.TableIsn := t.tableisn;
      select RowIsn bulk collect into vListRows from pcopy_rows 
       where processid = pProcessID 
         and ownerisn = t.ownerisn 
         and tableisn = t.tableisn
         and operation = t.operation;
      begin
        PerfomActionOnRows(vRowInfo, vListRows, t.operation);
        if vSkipError then
          commit;
        end if;
      exception
        when others then
          if vSkipError then
            rollback;
          else
            raise;
          end if;
      end blockf;
    end loop;
    
    -- Включаем все триггеры, выполнится commit для всех изменений
    TriggersEnable;
  exception 
    when others then
      rollback;
      TriggersEnable;
      Raise;
  end;

  procedure AddToCopyListWithOperation(pRow tRowInfo, pOperation char) is
  begin
    insert into pcopy_rows (processid, ownerisn, tableisn, rowisn, operation, status)
    values (vProcessID, pRow.OwnerIsn, pRow.TableIsn, pRow.Isn, pOperation, '0');
  end;
  
  procedure AddToDeleteList(pRow tRowInfo, pQuery varchar2) is
    cur sys_refcursor;
    vRow tRowInfo := pRow;
  begin
    open cur for pQuery;
    loop
      fetch cur into vRow.Isn;
      exit when cur%notfound;
      AddToCopyListWithOperation(vRow, 'D');
    end loop;
    close cur;
  exception
    when others then 
      close cur;
      raise;
  end;
  
  procedure AddToCopyList(pRow tRowInfo) is
  begin
    if IsIsnExist(pRow, true) then
      if IsIsnExist(pRow) then
        if vCopyArgs.DoUpdate then
          AddToCopyListWithOperation(pRow, 'U');
        elsif vCopyArgs.DoDelete then
          AddToCopyListWithOperation(pRow, 'D');
          AddToCopyListWithOperation(pRow, 'I');
        else
          null; -- Запись есть и тут и там, настроек нет. Не копируем, не обновляем.
        end if;
      else
        AddToCopyListWithOperation(pRow, 'I');
      end if;
    else
      null; -- Отсутствует запись по дблинку, ничего не делаем      
    end if;
  end;
  
  procedure PrepareRow(pRow tRowInfo);
  
  procedure PerformLink(pRow tRowInfo, pLink pcopy_links%rowtype) is
    vBaseIsn number := pRow.Isn;
    vQuery varchar2(4000);
    vQueryIsnsByDbLink varchar2(4000);
    vRow tRowInfo := pRow;
    cur sys_refcursor;
  begin
   /* Объект pRow относится к таблице Base из pRow. В pRow всегда хранится ISN строки
    * Если поле(BaseField) по которому связана запись с таблицей Link не 'ISN'
    * То нам нужно получить числовое значение этого поля..
    */
    if locdictname(pLink.Basefieldisn) <> 'ISN' then
      vQuery   := 'select ' || locdictname(pLink.Basefieldisn) || ' into :a from ' ||
                  GetTableName(pRow, true) || ' where isn=' || pRow.Isn;
      vBaseIsn := SafeExecute(vQuery); 
    end if;
    
    vRow.TableIsn := pLink.Linktableisn;
    vRow.OwnerIsn := pLink.Linkownerisn;
    vQueryIsnsByDbLink := 'select isn from '||GetTableName(vRow, true) ||' where ' || locdictname(pLink.Linkfieldisn) || '=' || vBaseIsn;

    if vCopyArgs.DoDelete then
      /* Ищем строки на удаление, у которых нет аналогов по dblink. 
       * Пример: На ПС у договора 15 условий, на сервере по дблинк - 10 условий. Мы удаляем 5 условий сейчас, остальные 10 обработаются стандартно.
       */      
      vQuery := 'select isn from ' || GetTableName(vRow) || ' t where ' || locdictname(pLink.Linkfieldisn) || '=' || vBaseIsn
                || ' and t.isn not in ('|| vQueryIsnsByDbLink ||')';
      AddToDeleteList(vRow, vQuery);
    end if;    
    
    open cur for vQueryIsnsByDbLink;
    loop
      fetch cur into vBaseIsn;
      exit when cur%notfound;
      vRow.Isn := vBaseIsn;
      PrepareRow(vRow);
    end loop;
    close cur;
  exception 
    when others then 
      if cur%isopen then close cur; end if;        
  end;
  
  procedure PrepareRow(pRow tRowInfo) is
  begin
    -- Заносим строку в список на копирование
    AddToCopyList(pRow);
  
    -- Ищем связи
    for link in (select *
                   from pcopy_links
                  where baseownerisn = pRow.OwnerIsn
                    and basetableisn = prow.TableIsn
                    and linktype <> 'D') loop
      PerformLink(pRow, link);
    end loop;
  end;

  procedure CopyObj(pCopyArgs TCopyArgs) is
    vOwnerIsn number := GetObjIsn(nvl(pCopyArgs.OwnerName, 'ais'));
    vTableIsn number := GetObjIsn(pCopyArgs.TableName);
    vRowInfo tRowInfo;
  begin
    vProcessID := SEQPCOPYPROCESS.Nextval;
    vCopyArgs := pCopyArgs;
    vRowInfo.Isn := pCopyArgs.Isn;
    vRowInfo.TableIsn := vTableIsn;
    vRowInfo.OwnerIsn := vOwnerIsn;

    PrepareRow(vRowInfo);
    ProcessRows(vProcessID);
  end;
  
begin
  InitDictionary;
end pcopy;
/
";
    }
}
