-- FUNCTION: public.get_listitems()

-- DROP FUNCTION IF EXISTS public.get_listitems();

CREATE OR REPLACE FUNCTION public.get_listitems(
	)
    RETURNS TABLE(categoryid integer, categoryname character varying, subcategoryid integer, subcategoryname character varying, productid integer, productname character varying, productdescription text, originalproductprice numeric, offerproductprice numeric) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN

RETURN QUERY (
SELECT C.CategoryId,C.CategoryName,SC.SubCategoryId,SC.SubCategoryName,
	P.ProductId,P.ProductName,P.Description,P.OriginalPrice,P.OfferPrice
	FROM Category C
	INNER JOIN SubCategory SC ON C.CategoryId = SC.CategoryId
	INNER JOIN Products P ON SC.SubCategoryId = P.SubCategoryId
);

END;
$BODY$;

