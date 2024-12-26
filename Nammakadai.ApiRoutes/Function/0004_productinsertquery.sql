INSERT INTO public.category( categoryname, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Grocery', true, false, null, current_timestamp, null, current_timestamp);
	
INSERT INTO public.category( categoryname, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Beauty and Personal Care',true, false, null, current_timestamp, null, current_timestamp);
	
INSERT INTO public.category( categoryname, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Household &Essentials',true, false, null, current_timestamp, null, current_timestamp);

INSERT INTO public.category( categoryname, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Snacks and Drinks', true, false, null, current_timestamp, null, current_timestamp);
-----------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Fresh Fruits', 1, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Rice', 1, true, false, null, current_timestamp,null,current_timestamp);
	
INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Bread & Eggs', 1, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Fresh Vegetables', 1, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Masala', 1, true, false, null, current_timestamp,null,current_timestamp);
-----------------------------------------------------------------------
INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Cold Drinks & Juices', 4, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Biscuits&Cookies', 4, true, false, null, current_timestamp,null,current_timestamp);
-----------------------------------------------------------------

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('SkinCare', 2, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Bath & Body', 2, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Fragrances & Grooming', 2, true, false, null, current_timestamp,null,current_timestamp);

---------------------------------------------------------------------------------------------------------
INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Cleaning & Essentials', 3, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.subcategory(
subcategoryname, categoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
VALUES ('Home needs', 3, true, false, null, current_timestamp,null,current_timestamp);

------------------------------------------------------------------------------------------------------------------

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Apple', null, 40, 35, 100, 1, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Orange', null, 45, 30, 100, 1, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Onion', null, 40, 35, 100, 4, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Tomatto', null, 45, 30, 100, 4, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Assairvad', null, 40, 35, 100, 2, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('ponni rice', null, 45, 30, 100, 2, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Bread', null, 30, 40, 100, 3, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Eggs', null, 5, 6, 100, 3, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('oil', null, 30, 40, 100, 7, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Hammam', null, 25, 30, 100, 8, true, false, null, current_timestamp,null,current_timestamp);

INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('7up', null, 30, 40, 100, 5, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('MilkBikis', null, 25, 30, 100, 6, true, false, null, current_timestamp,null,current_timestamp);
INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('Fogg', null, 30, 40, 100, 9, true, false, null, current_timestamp,null,current_timestamp);
	INSERT INTO public.products(
	 productname, description, originalprice, offerprice, stockquantity, subcategoryid, isactive, isdeleted, createdby, createdon, modifiedby, modifiedon)
	VALUES ('harpics', null, 25, 30, 100, 10, true, false, null, current_timestamp,null,current_timestamp);
	
	
	
