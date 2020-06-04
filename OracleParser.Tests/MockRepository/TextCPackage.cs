using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class TextCPackage
    {
        public const string Spec =
@"CREATE OR REPLACE PACKAGE c_package AS 

   type shit is record(a number);
   lol kk.shit;
   type shitt is table of shit index by pls_integer;
   varib varchar2(220);

   cursor cur is select * from dual;
   vur2 sys_refcursor;

   -- Adds a customer 
   PROCEDURE addCustomer(c_id   customers%type, 
   c_name  customerS.No.ame%type, 
   c_age  customers.age%type, 
   c_addr customers.address%type,  
   c_sal  customers.salary%type); 
   
   -- Removes a customer 
   PROCEDURE delCustomer(c_id  customers%TYPE); 

   PROCEDURE listCustomer; 
  
END c_package; 
/";

        public const string Body =
@"CREATE OR REPLACE PACKAGE BODY c_package AS 

   PackageVariable number := 10;

   type PackageType is table of number;

   PROCEDURE addCustomer(c_id  customers%type, 
      c_name customerS.No.ame%type, 
      c_age  customers.age%type, 
      c_addr  customers.address%type,  
      c_sal   customers.salary%type) 
   IS 
   BEGIN 
      INSERT INTO customers (id,name,age,address,salary) 
         VALUES(c_id, c_name, c_age, c_addr, c_sal); 
   END addCustomer; 

   function test(a in number, b in number) return varchar2 is
    begin
    return '123';
    end;

   function SumOfTwo(a in number, 
                     b in number) 
            return number is
     vRes number;
    begin
    vRes := a + b;
    return vRes;
    end;
   
   PROCEDURE delCustomer(c_id   customers%type) IS 
   BEGIN 
      DELETE FROM customers 
      WHERE id = c_id; 
   END delCustomer;  
   
   /* очепушительный комент
    * к процедуре
    * listCustomer
    */
   PROCEDURE listCustomer IS 
   CURSOR c_customers is 
      SELECT  name FROM customers; 
   TYPE c_list is TABLE OF customers.Name%type; 
   name_list c_list := c_list(); 
   counter integer :=0; 
   BEGIN 
      FOR n IN c_customers LOOP 
      counter := counter +1; 
      name_list.extend; 
      name_list(counter) := n.name; 
      dbms_output.put_line('Customer(' ||counter|| ')'||name_list(counter)); 
      END LOOP; 
   END listCustomer;
   
END c_package; 
/";
    }
}
