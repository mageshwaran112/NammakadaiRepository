-- FUNCTION: public.get_userbyid(integer)

-- DROP FUNCTION IF EXISTS public.get_userbyid(integer);

CREATE OR REPLACE FUNCTION public.get_userbyid(
	user_id integer)
    RETURNS TABLE(userid integer, username character varying, usermail character varying, phonenumber character varying) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
BEGIN
    RETURN QUERY
    SELECT 
        u.Id AS UserId,
        u.UserName,
        u.UserMail,
        u.PhoneNumber
    FROM 
        Users u
    WHERE 
        u.Id = User_ID
    ORDER BY 
        u.Id;
END;
$BODY$;

ALTER FUNCTION public.get_userbyid(integer)
    OWNER TO postgres;
