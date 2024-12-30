-- FUNCTION: public.getcartproducts(integer)

-- DROP FUNCTION IF EXISTS public.getcartproducts(integer);

--SELECT * FROM getcartproducts(1)

CREATE OR REPLACE FUNCTION public.getcartproducts(
    in_userid INTEGER
)
RETURNS JSONB
LANGUAGE 'plpgsql'
COST 100
VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE 
    ResultSet JSONB;
    TotalPrice NUMERIC;
BEGIN
   
    SELECT SUM(
        CASE 
            WHEN P.OfferPrice IS NOT NULL THEN P.OfferPrice 
            ELSE P.OriginalPrice 
        END * C.Quantity
    )
    INTO TotalPrice
    FROM public.products P
    INNER JOIN public.cart C ON P.ProductId = C.ProductId
    WHERE C.CreatedBy = in_userid AND C.IsDeleted = FALSE;

    SELECT 
        json_build_object(
           'cartdetails', 
           (SELECT jsonb_agg(row_to_json(cart))
            FROM (
                SELECT 
                    C.CartId,
                    P.ProductId,
                    P.ProductName,
                    P.Description,
                    P.OriginalPrice,
                    P.OfferPrice,
                    CASE WHEN P.StockQuantity > 0 THEN TRUE ELSE FALSE END AS StockAvailability,
                    C.Quantity
                FROM public.products P
                INNER JOIN public.cart C ON P.ProductId = C.ProductId
                WHERE C.CreatedBy = in_userid AND C.IsDeleted = FALSE
            ) cart),
           'totalprice', TotalPrice
        )
    INTO ResultSet;

    RETURN ResultSet;
END;
$BODY$;

