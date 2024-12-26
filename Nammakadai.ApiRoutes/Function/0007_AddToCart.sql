-- FUNCTION: public.add_to_cart(integer, integer, integer)

-- DROP FUNCTION IF EXISTS public.add_to_cart(integer, integer, integer);

CREATE OR REPLACE FUNCTION public.add_to_cart(
	in_productid integer,
	in_quantity integer,
	in_userid integer)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN

IF EXISTS (SELECT 1 FROM public.cart WHERE ProductId = In_ProductId AND CreatedBy = In_UserId AND IsDeleted = False)
THEN
UPDATE public.cart SET Quantity = Quantity + In_Quantity, ModifiedBy = In_UserId,ModifiedOn = NOW() 
WHERE CreatedBy = In_UserId AND ProductId = In_ProductId;
ELSE
INSERT INTO public.cart (ProductId,Quantity,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsActive,IsDeleted)
VALUES (In_ProductId,In_Quantity,In_UserId,NOW(),In_UserId,NOW(),TRUE,FALSE);
END IF;
END;
$BODY$;
