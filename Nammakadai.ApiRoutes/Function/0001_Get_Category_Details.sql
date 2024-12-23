--DROP FUNCTION Get_Category_Details()
CREATE OR REPLACE FUNCTION Get_Category_Details()
RETURNS TABLE (category_result JSON, subcategory_result JSON, product_result JSON) AS $$
BEGIN
    RETURN QUERY 
    SELECT 
        (SELECT json_agg(row_to_json(c)) FROM
		(SELECT categoryid,categoryname,isactive FROM Category) c) AS category_result,
        (SELECT json_agg(row_to_json(sc)) FROM 
		(SELECT subcategoryid,subcategoryname,categoryid FROM SubCategory)sc) AS subcategory_result,
        (SELECT json_agg(row_to_json(p)) FROM
		(SELECT productid,productname,originalprice,offerprice,stockquantity,subcategoryid FROM Products)p) AS product_result;
END;
$$ LANGUAGE plpgsql;