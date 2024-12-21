-- FUNCTION: public.saveuserotp(integer, text)

-- DROP FUNCTION IF EXISTS public.saveuserotp(integer, text);

CREATE OR REPLACE FUNCTION public.saveuserotp(
	in_userid integer,
	in_otp text)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
    IF EXISTS(SELECT 1 FROM UserDetail WHERE UserId = In_UserId) THEN
        UPDATE UserDetails SET 
		UserId = In_UserId,
		OTP = In_OTP,
		ModifiedBy = In_UserId,
		ModifiedOn = NOW();
    ELSE
        INSERT INTO UserDetail (UserId, IsLoggedIn, OTP, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn,IsDeleted)
        VALUES (In_UserId,TRUE, In_OTP,In_UserId, NOW(), In_UserId, NOW(), FALSE);
    END IF;
END;
$BODY$;
