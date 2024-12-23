--DROP FUNCTION Get_Category_Details()
CREATE OR REPLACE FUNCTION public.get_category_details()
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
    combined_result jsonb;
BEGIN
    SELECT jsonb_build_object(
        'categories', (SELECT jsonb_agg(row_to_json(cat)) FROM (SELECT categoryid, categoryname, isactive FROM Category) cat),
        'subcategories', (SELECT jsonb_agg(row_to_json(sub)) FROM (SELECT subcategoryid, subcategoryname, categoryid FROM SubCategory) sub),
        'products', (SELECT jsonb_agg(row_to_json(pro)) FROM (SELECT productid, productname, originalprice, offerprice, stockquantity, subcategoryid FROM Products) pro)
    ) INTO combined_result;
 
    RETURN combined_result;
END;
$BODY$;