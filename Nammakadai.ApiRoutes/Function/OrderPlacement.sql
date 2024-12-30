-- FUNCTION: public.orderplacement(text)

-- DROP FUNCTION IF EXISTS public.orderplacement(text);

CREATE OR REPLACE FUNCTION public.orderplacement(
	ip_payload text)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE product JSONB;
  user_id INT;
  orderrequest jsonb;
BEGIN
user_id := (orderrequest ->> 'UserId')::INT;
orderrequest := ip_payload;
	
FOR product IN SELECT * FROM jsonb_array_elements(OrderRequest -> 'ProductDetails')LOOP 
INSERT INTO public.Order (ProductId,Quantitiy,ProductPrice,OverallPrice,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)
VALUES ((product ->> 'ProductIds') :: INT,
	    (product ->> 'Quantities') :: INT,
		(product ->> 'Price') :: NUMERIC,
	    (product ->> 'Price' ) :: Numeric * (product ->> 'Quantities') :: NUMERIC,
	    (user_id),
	    Now(),
	    user_id,
	    Now());
END LOOP;
END;
$BODY$;

