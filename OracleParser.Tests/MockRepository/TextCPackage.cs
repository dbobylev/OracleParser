﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OracleParser.Tests.MockRepository
{
    class TextCPackage
    {
        public const string Spec =
@"CREATE OR REPLACE PACKAGE c_package AS 
   -- Adds a customer 
   PROCEDURE addCustomer(c_id   customers.ider%type, 
   c_name  customerS.No.ame%type, 
   c_age  customers.age%type, 
   c_addr customers.address%type,  
   c_sal  customers.salary%type); 
   
   -- Removes a customer 
   PROCEDURE delCustomer(c_id  customers.ider%TYPE); 
   --Lists all customers 
   function sum(a number) return sys_refcursor; 

   PROCEDURE listCustomer; 
  
END c_package; 
/";

        public const string Body =
@"CREATE OR REPLACE PACKAGE BODY c_package AS 
   PROCEDURE addCustomer(c_id  customers.id%type, 
      c_name customerS.No.ame%type, 
      c_age  customers.age%type, 
      c_addr  customers.address%type,  
      c_sal   customers.salary%type) 
   IS 
   BEGIN 
      INSERT INTO customers (id,name,age,address,salary) 
         VALUES(c_id, c_name, c_age, c_addr, c_sal); 
   END addCustomer; 
   
   PROCEDURE delCustomer(c_id   customers.id%type) IS 
   BEGIN 
      DELETE FROM customers 
      WHERE id = c_id; 
   END delCustomer;  
   
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
