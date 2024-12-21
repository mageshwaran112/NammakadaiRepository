-- FUNCTION: public.registeruser(character varying, character varying, character varying)

-- DROP FUNCTION IF EXISTS public.registeruser(character varying, character varying, character varying);

CREATE OR REPLACE FUNCTION public.registeruser(
	in_username character varying,
	in_mailid character varying,
	in_phonenumber character varying)
    RETURNS boolean
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
BEGIN
    INSERT INTO USERS (UserName, UserMail, PhoneNumber, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, IsActive, IsDeleted)
    VALUES (In_UserName, In_MailId, In_PhoneNumber, NULL, NOW(), NULL, NOW(), 1, 0);
    
    RETURN TRUE;
END;
$BODY$;
