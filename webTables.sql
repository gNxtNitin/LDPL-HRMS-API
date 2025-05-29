-- Drop procedures if exist
BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_GetAuthenticatedUser';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_GetSetMenu';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_log_audit_trail';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

-- Drop tables if exist (drop child tables first due to FK dependencies)
BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_USERROLES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLEMENUFEATUREACCESS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLEMENUACCESS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUFEATURES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUFEATUREMASTER CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_USERS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_CONFIGURATION CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_AUDITTRAIL CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/


BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_PasswordResetTokens CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_EmailNotification CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE G_EMP_DA_BILLS CASCADE CONSTRAINTS';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE G_EMP_DA CASCADE CONSTRAINTS';
EXCEPTION
    WHEN OTHERS THEN NULL;
END;
/



COMMIT;

-- Create tables

-- 1. Audit Trail
CREATE TABLE G_AUDITTRAIL
(
  ID             NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  TABLE_NAME     VARCHAR2(255 BYTE),
  RECORD_ID      VARCHAR2(255 BYTE),
  OPERATION      VARCHAR2(10 BYTE)              DEFAULT NULL,
  CHANGED_DATA   CLOB                           DEFAULT NULL,
  PREVIOUS_DATA  CLOB                           DEFAULT NULL,
  CHANGED_BY     VARCHAR2(255 BYTE)             DEFAULT NULL,
  CHANGED_AT     TIMESTAMP(6)                   DEFAULT CURRENT_TIMESTAMP,
  IP_ADDRESS     VARCHAR2(45 BYTE)              DEFAULT NULL,
  REMARKS        CLOB                           DEFAULT NULL
);

-- 2. Configuration (no primary key or unique specified)
CREATE TABLE G_CONFIGURATION
(
  CONFIG_KEY    VARCHAR2(255 BYTE),
  CONFIG_VALUE  VARCHAR2(255 BYTE)
);

-- 3. Menu Feature Master
CREATE TABLE G_MENUFEATUREMASTER
(
  FEATUREID           NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  FEATURENAME         VARCHAR2(100 BYTE),
  FEATURECODE         VARCHAR2(50 BYTE),
  FEATUREDESCRIPTION  VARCHAR2(255 BYTE),
  CREATEDBY           NUMBER(10),
  CREATEDDATE         DATE,
  MODIFIEDBY          NUMBER(10),
  MODIFIEDDATE        DATE
);

-- 5. Menus (create before G_MENUFEATURES as FK references it)
CREATE TABLE G_MENUS
(
  MENUID          NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  PARENTID        NUMBER(10)                    DEFAULT 0,
  MENUNAME        NVARCHAR2(100),
  AREA            NVARCHAR2(50),
  CONTROLLERNAME  NVARCHAR2(50),
  ACTIONNAME      NVARCHAR2(50),
  URL             VARCHAR2(255),
  "Order"         NUMBER(10),
  ISACTIVE        NUMBER(1)                     DEFAULT 0,
  ICONCLASS       VARCHAR2(30 BYTE),
  CREATEDBY       NUMBER(10)                    DEFAULT 0,
  CREATEDDATE     DATE,
  MODIFIEDBY      NUMBER(10),
  MODIFIEDDATE    DATE
);

-- 4. Menu Features (depends on G_MENUS and G_MENUFEATUREMASTER)
CREATE TABLE G_MENUFEATURES
(
  MENUFEATUREID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  MENUID         NUMBER,
  FEATUREID      NUMBER,
  CREATEDBY      NUMBER,
  CREATEDDATE    DATE                           DEFAULT SYSDATE,
  MODIFIEDBY     NUMBER,
  MODIFIEDDATE   DATE,
  FEATUREURL     VARCHAR2(255 BYTE),
  CONSTRAINT FK_MENUFEATURES_MENUID FOREIGN KEY (MENUID) REFERENCES G_MENUS(MENUID),
  CONSTRAINT FK_MENUFEATURES_FEATUREID FOREIGN KEY (FEATUREID) REFERENCES G_MENUFEATUREMASTER(FEATUREID)
);

-- 8. Roles
CREATE TABLE G_ROLES
(
  ROLEID        NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLENAME      NVARCHAR2(100),
  DESCRIPTION   NVARCHAR2(255),
  ISACTIVE      NUMBER(1)                       DEFAULT 1,
  CREATEDBY     NUMBER(10),
  CREATEDDATE   DATE                            DEFAULT CURRENT_DATE,
  MODIFIEDBY    NUMBER(10),
  MODIFIEDDATE  DATE,
  CONSTRAINT UQ_G_ROLES_ROLENAME UNIQUE (ROLENAME)
);

-- 9. Users
CREATE TABLE G_USERS
(
  USERID                   NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  CUSTOMERID               VARCHAR2(50 BYTE),
  COMPANYCODE              NUMBER(2),
  STATUS                   VARCHAR2(2 BYTE),
  FIRSTNAME                VARCHAR2(65 BYTE),
  LASTNAME                 VARCHAR2(50 BYTE),
  USERNAME                 NVARCHAR2(100),
  EMAIL                    NVARCHAR2(100),
  MOBILE                   VARCHAR2(15),
  DOB                      DATE,
  PASSWORDHASH             NVARCHAR2(255),
  OTP                      VARCHAR2(50),
  OTPCREATEDDATE           DATE,
  ISRESENDOTP              NUMBER(1),
  RESENDOTPDATE            DATE,
  LASTLOGINDATE            DATE,
  ISFIRSTTIMELOGIN         NUMBER(1),
  PASSWORDLASTCHANGEDDATE  DATE,
  ISACTIVE                 NUMBER(1)            DEFAULT 1,
  CREATEDBY                NUMBER(10),
  CREATEDDATE              TIMESTAMP(6)         DEFAULT CURRENT_TIMESTAMP,
  MODIFIEDBY               NUMBER(10),
  MODIFIEDDATE             TIMESTAMP(6),
  FILEPATH                 NVARCHAR2(255),
  CUSTOMERTYPE             CHAR(1 BYTE),
  CONSTRAINT UQ_G_USERS_USERNAME UNIQUE (USERNAME),
  CONSTRAINT UQ_G_USERS_EMAIL UNIQUE (EMAIL),
  CONSTRAINT UQ_G_USERS_CUSTOMERID UNIQUE (CUSTOMERID)
);

-- 6. Role Menu Access (depends on G_ROLES and G_MENUS)
CREATE TABLE G_ROLEMENUACCESS
(
  ROLEMENUACCESSID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLEID            NUMBER(10),
  MENUID            NUMBER(10),
  CREATEDBY         NUMBER(10),
  CREATEDDATE       DATE                        DEFAULT CURRENT_DATE,
  MODIFIEDBY        NUMBER(10),
  MODIFIEDDATE      DATE,
  ISACCESSIBLE      NUMBER(1)                   DEFAULT 1,
  CONSTRAINT FK_ROLEMENUACCESS_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID),
  CONSTRAINT FK_ROLEMENUACCESS_MENUID FOREIGN KEY (MENUID) REFERENCES G_MENUS(MENUID)
);

-- 7. Role Menu Feature Access (depends on G_ROLES and G_MENUFEATURES)
CREATE TABLE G_ROLEMENUFEATUREACCESS
(
  ROLEMENUFEATUREACCESSID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLEID                   NUMBER(10),
  MENUFEATUREID            NUMBER(10),
  ISACTIVE                 NUMBER(1)            DEFAULT 1,
  CREATEDBY                NUMBER(10),
  CREATEDDATE              DATE                 DEFAULT SYSDATE,
  MODIFIEDBY               NUMBER(10),
  MODIFIEDDATE             DATE,
  CONSTRAINT FK_ROLEMENUFEATURE_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID),
  CONSTRAINT FK_ROLEMENUFEATURE_MENUFEATUREID FOREIGN KEY (MENUFEATUREID) REFERENCES G_MENUFEATURES(MENUFEATUREID)
);

-- 10. User Roles (one role per user constraint)
CREATE TABLE G_USERROLES
(
  USERID        NUMBER(10) PRIMARY KEY,
  ROLEID        NUMBER(10),
  ASSIGNEDDATE  DATE                            DEFAULT SYSDATE,
  CONSTRAINT FK_USERROLES_USERID FOREIGN KEY (USERID) REFERENCES G_USERS(USERID) ON DELETE CASCADE,
  CONSTRAINT FK_USERROLES_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID) ON DELETE CASCADE
);


-- 11. Password Reset Tokens
CREATE TABLE G_PasswordResetTokens (
    TokenID            NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    UserID             NUMBER NOT NULL,
    UserName           VARCHAR2(20 CHAR) NOT NULL,
    ResetTokenHash     VARCHAR2(255 CHAR) NOT NULL,
    ExpirationTimeUTC  TIMESTAMP WITH TIME ZONE NOT NULL,
    IsUsed             NUMBER(1) DEFAULT 0 CHECK (IsUsed IN (0, 1)),
    CreatedDate        TIMESTAMP WITH TIME ZONE DEFAULT SYSTIMESTAMP AT TIME ZONE 'UTC',

    -- Constraints
    CONSTRAINT uq_ResetTokenHash UNIQUE (ResetTokenHash),
    CONSTRAINT fk_UserID FOREIGN KEY (UserID)
        REFERENCES G_Users (UserID)
        ON DELETE CASCADE
);

-- 12. Email Notifications 
CREATE TABLE G_EmailNotification (
    ID              NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Token           VARCHAR2(255 CHAR),
    ToEmailIds      VARCHAR2(1024 CHAR),
    EmailSubject    VARCHAR2(255 CHAR),
    EmailBody       CLOB,
    HasAttachment   NUMBER(1) DEFAULT 0 CHECK (HasAttachment IN (0, 1)),
    IsSent          NUMBER(1) DEFAULT 0 CHECK (IsSent IN (0, 1)),
    CreatedDate     TIMESTAMP(6),
    SentDate        TIMESTAMP(6),
    Status          CHAR(1 CHAR),
    IsHTML          NUMBER(1) DEFAULT 0 CHECK (IsHTML IN (0, 1))
);


-- 13. DA records
-- Create G_EMP_DA table
CREATE TABLE G_EMP_DA (
  DAID             VARCHAR2(36 BYTE) DEFAULT SYS_GUID() NOT NULL,
  EMPID            VARCHAR2(100 BYTE) NOT NULL,
  DA               NUMBER(10,2) NOT NULL,
  HOTEL            NUMBER(10,2),
  OTHER            NUMBER(10,2),
  KM               NUMBER(10,2),
  FROMDATE         DATE NOT NULL,
  TODATE           DATE NOT NULL,
  DASTATUS         CHAR(3 BYTE) DEFAULT 'NO',
  ADDDATETIME      TIMESTAMP(6),
  ARDATETIME       TIMESTAMP(6),
  APPROVEREJECTBY  VARCHAR2(50 BYTE),
  CONSTRAINT CHK_KM_NON_NEGATIVE CHECK (KM >= 0),
  CONSTRAINT PK_G_EMP_DA PRIMARY KEY (DAID)
);

-- Create G_EMP_DA_BILLS table
CREATE TABLE G_EMP_DA_BILLS (
  BILL_ID         NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  DAID            VARCHAR2(36 BYTE) NOT NULL,
  BILL_FILE_NAME  VARCHAR2(255 BYTE),
  DESCRIPTION     VARCHAR2(4000 BYTE),
  ADDEDDATE       DATE DEFAULT SYSDATE
);

-- Add foreign key constraint
ALTER TABLE G_EMP_DA_BILLS
ADD CONSTRAINT FK_G_EMP_DA_BILLS_DAID
FOREIGN KEY (DAID)
REFERENCES G_EMP_DA (DAID)
ON DELETE CASCADE;

COMMIT;


CREATE OR REPLACE PROCEDURE G_SP_GetAuthenticatedUser (
    p_userid        IN VARCHAR2,
    p_password      IN VARCHAR2,
    ret             OUT NUMBER,
    errormsg        OUT VARCHAR2,
    p_result        OUT SYS_REFCURSOR
) AS
    v_UserId            G_Users.UserId%TYPE;
    v_IsFirstTimeLogin  G_Users.IsFirstTimeLogin%TYPE;
BEGIN
    -- Initialize outputs
    errormsg := 'Something went wrong!';
    ret := -1;

    BEGIN
        -- Try fetching the user details
        SELECT U.UserId, U.IsFirstTimeLogin
        INTO v_UserId, v_IsFirstTimeLogin
        FROM G_Users U
        WHERE U.UserName = p_userid
          AND U.PasswordHash = p_password
          AND U.IsActive = 1
          AND ROWNUM = 1;

        -- Success: determine response based on IsFirstTimeLogin
        IF v_IsFirstTimeLogin is null or v_IsFirstTimeLogin = 1 THEN
            ret := 201; -- First time login
        ELSE
            ret := 200; -- Authenticated
        END IF;

        errormsg := 'User authenticated successfully.';

        -- Return user roles and metadata
        OPEN p_result FOR
            SELECT 
                U.UserID,
                R.RoleName,
                U.UserName,
                UR.RoleID,
                U.Mobile,
                U.Email,
                U.LastLoginDate
            FROM G_Users U
            JOIN G_UserRoles UR ON U.UserId = UR.UserID
            JOIN G_Roles R ON UR.RoleId = R.RoleId
            WHERE U.UserId = v_UserId;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            errormsg := 'Invalid username or password.';
            ret := -1; -- Unauthorized
        WHEN OTHERS THEN
            errormsg := 'Unexpected error: ' || SQLERRM;
            ret := -1; -- Internal error
    END;

END;
/


CREATE OR REPLACE PROCEDURE G_sp_GetSetMenu(
    p_flag             IN Char DEFAULT 'G',
    p_MenuId           IN NUMBER DEFAULT 0,
    
    p_UserId           IN NUMBER DEFAULT 0,
    ret             OUT VARCHAR2,
    errormsg        OUT VARCHAR2,
    --p_Flag            IN  VARCHAR2 DEFAULT NULL,
  
    g_ResultSet       OUT SYS_REFCURSOR
)
AS
BEGIN
    IF p_flag = 'I' THEN
        
        OPEN g_ResultSet FOR
        SELECT TO_CHAR(M.MenuId) as MenuId, TO_CHAR(M.ParentId) as ParentId, M.MenuName, M.Area, M.ControllerName,
               M.ActionName, M.Url, TO_CHAR(M."Order") as  "Order",
                TO_CHAR(M.IsActive) as IsActive,
                
                M.IconClass
        FROM g_Users U
        JOIN g_UserRoles UR ON U.UserId = UR.UserId
        JOIN g_RoleMenuAccess RM ON RM.RoleId = UR.RoleId
        JOIN g_Menus M ON M.MenuId = RM.MenuId
        WHERE U.UserId = p_UserId AND M.IsActive = 1 AND RM.IsAccessible = 1
        order by "Order";
 
        ret := 200;
        errormsg := 'Menu Fetched successfully';
 
    ELSE -- Default case: G
        OPEN g_ResultSet FOR
        SELECT M.MenuId, M.ParentId, M.MenuName, M.Area, M.ControllerName,
               M.ActionName, M.Url, M."Order", M.IsActive, M.IconClass
        FROM g_Menus M
        ORDER BY M.MenuId, M."Order";
 
        ret := 200;
        errormsg := 'Menu Fetched successfully';
    END IF;
 
END G_sp_GetSetMenu;
/


CREATE OR REPLACE PROCEDURE G_sp_log_audit_trail (
    p_table_name     IN VARCHAR2,
    p_record_id      IN VARCHAR2,
    p_operation      IN VARCHAR2,
    p_changed_data   IN CLOB DEFAULT NULL,
    p_previous_data  IN CLOB DEFAULT NULL,
    p_changed_by     IN VARCHAR2 DEFAULT USER,
    p_ip_address     IN VARCHAR2 DEFAULT NULL,
    p_remarks        IN CLOB DEFAULT NULL
) AS
BEGIN
    
    DECLARE
        v_enabled VARCHAR2(10);
    BEGIN
        SELECT config_value INTO v_enabled
        FROM g_configuration
        WHERE config_key = 'AuditLogEnabled';

        IF v_enabled = '0' THEN
            RETURN;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            NULL; 
    END;

    
    INSERT INTO g_audittrail (
        table_name,
        record_id,
        operation,
        changed_data,
        previous_data,
        changed_by,
        ip_address,
        remarks
    ) VALUES (
        p_table_name,
        p_record_id,
        UPPER(p_operation),
        p_changed_data,
        p_previous_data,
        p_changed_by,
        p_ip_address,
        p_remarks
    );
    commit;
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END;
/

CREATE OR REPLACE PROCEDURE G_SP_GetSetEmailNotification (
    p_flag          IN  CHAR,             -- 'C' = Set (insert), 'G' = Get by Token, 'U' - UPDATE
    p_token         IN  VARCHAR2,
    p_toemailids    IN  VARCHAR2 DEFAULT NULL,
    p_subject       IN  VARCHAR2 DEFAULT NULL,
    p_body          IN  CLOB DEFAULT NULL,
    p_hasattachment IN Number Default 0,
    P_issent        IN Number default 0,
    p_status        IN CHAR default 'A',
    p_ishtml        IN  NUMBER DEFAULT 0,
    ret           OUT NUMBER,
    errormsg      OUT VARCHAR2,
    p_result        OUT SYS_REFCURSOR

) AS
BEGIN
    ret := -1;
    errormsg := '';

    IF p_flag = 'C' THEN
    
       
        INSERT INTO G_EmailNotification (
            Token, ToEmailIds, EmailSubject, EmailBody,
            HasAttachment, IsSent, CreatedDate, Status, IsHTML
        ) VALUES (
            p_token, p_toemailids, p_subject, p_body,
            p_hasattachment, p_issent, SYSTIMESTAMP, p_status, p_ishtml
        );

        ret := 1;
        errormsg := 'Reset token generated!';

    ELSIF p_flag = 'G' THEN
        OPEN p_result FOR
            SELECT Token, ToEmailIds, EmailSubject, EmailBody,
            HasAttachment, IsSent, CreatedDate, Status, IsHTML
            FROM G_EmailNotification
            WHERE Token = p_token AND IsSent = 0 AND STATUS = p_status AND  ToEmailIds IS NOT NULL AND LENGTH(TRIM(ToEmailIds)) > 0;

        ret := 200;
        
    ELSIF p_flag = 'U' THEN
        UPDATE G_EmailNotification
        SET IsSent = p_issent, CreatedDate = SYSTIMESTAMP, Status = p_status
        WHERE Token = p_token;
    ELSE
        errormsg := 'Invalid flag';
        ret := -1;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        errormsg := 'Unexpected error: ' || SQLERRM;
        ret := -1;
END G_SP_GetSetEmailNotification;
/


CREATE OR REPLACE PROCEDURE G_SP_SetValidatePasswordResetToken (
    p_flag          IN  CHAR DEFAULT 'V',
    P_userid        IN  VARCHAR2,
    p_tokenhash     IN  VARCHAR2,
    ret           OUT NUMBER,
    errormsg      OUT VARCHAR2

) AS
    v_UserId               NUMBER;
    v_UserName             VARCHAR2(50);
    v_Email                VARCHAR2(100);
    v_ExistingTokenTime    TIMESTAMP WITH TIME ZONE;
    v_TokenExpiryMinutes   NUMBER;
    v_IsValid              NUMBER := -1;
    v_NowUTC               TIMESTAMP WITH TIME ZONE := SYSTIMESTAMP AT TIME ZONE 'UTC';
BEGIN
    IF p_flag = 'C' THEN
        BEGIN
            -- Get active user ID
            SELECT UserID, USERNAME, EMAIL
            INTO v_UserId, v_UserName, v_Email
            FROM G_Users
            WHERE USERNAME = p_userid AND IsActive = 1
            FETCH FIRST 1 ROWS ONLY;
            
            
            IF v_Email is NULL or LENGTH(TRIM(v_Email)) =0 THEN
                ret := -404;
                errormsg := 'Email Not Found!';
                RETURN;
            END IF;

            BEGIN
                SELECT ExpirationTimeUTC INTO v_ExistingTokenTime
                FROM G_PasswordResetTokens
                WHERE USERNAME = v_UserName
                FETCH FIRST 1 ROWS ONLY;

                IF v_ExistingTokenTime IS NOT NULL AND 
                   v_ExistingTokenTime > v_NowUTC - INTERVAL '60' MINUTE THEN
                    ret := -2;
                    errormsg := 'Reset token already generated in the last 1 hour';
                    RETURN;
                END IF;
            EXCEPTION
                WHEN NO_DATA_FOUND THEN
                    NULL; -- No token found, continue
            END;

--            -- Get expiration duration
--            SELECT ResetTokenExpiryMinutes
--            INTO v_TokenExpiryMinutes
--            FROM PasswordPolicy
--            FETCH FIRST 1 ROWS ONLY;
            
            v_TokenExpiryMinutes:=60;

            -- Insert new token using UTC timestamp
            INSERT INTO G_PasswordResetTokens (
                UserID,UserName, ResetTokenHash, ExpirationTimeUTC, IsUsed, CreatedDate
            ) VALUES (
                v_UserId,
                v_UserName,
                p_TokenHash,
                v_NowUTC + INTERVAL '1' MINUTE * v_TokenExpiryMinutes,
                0,
                v_NowUTC
            );

            ret := v_UserId;
            errormsg := v_Email;
            
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                ret := -1;
                errormsg := 'User is not found';
            WHEN OTHERS THEN
                ret := -9;
                errormsg := 'Unexpected error (C): ' || SQLERRM;
        END;

    ELSIF p_flag = 'U' THEN
        -- Mark token as used
        UPDATE G_PasswordResetTokens
        SET IsUsed = 1
        WHERE ResetTokenHash = p_TokenHash;

        ret := 200;

    ELSIF p_flag = 'V' THEN
        BEGIN
            SELECT CASE 
                     WHEN ExpirationTimeUTC > v_NowUTC AND IsUsed = 0 THEN 1 
                     ELSE -1 
                   END
            INTO v_IsValid
            FROM G_PasswordResetTokens
            WHERE ResetTokenHash = p_TokenHash
            FETCH FIRST 1 ROWS ONLY;

            ret := v_IsValid;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                ret := -1;
                errormsg := 'Token not found or expired';
        END;

    ELSE
        ret := -1;
        errormsg := 'Invalid request';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        errormsg := 'Unexpected error: ' || SQLERRM;
        ret := -99;
END G_SP_SetValidatePasswordResetToken;
/



-- INSERT ROLES
INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('ADMIN', 'For Admin Role - Full Access', 1, 0, SYSDATE);

INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('SALESMANAGER', 'FOR Sales Manager Role', 1, 0, SYSDATE);

INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('CSR', 'For CSR (Rider) Role', 1, 0, SYSDATE);

commit;


CREATE OR REPLACE PROCEDURE G_SP_GetSetDailyEPunch(
    p_flag        IN CHAR DEFAULT 'G',
    p_empId       IN VARCHAR2,
    p_lattitude   IN NUMBER,
    p_longitude   IN NUMBER,
    p_ephoto      IN VARCHAR2,
    p_km          IN NUMBER,
    p_address       IN VARCHAR2,
    p_location    IN VARCHAR2,
    p_schoolId      IN VARCHAR2,
    p_isaddressmatched IN NUMBER,
   
    p_fromDate    IN DATE,
    p_toDate      IN DATE,

    ret           OUT VARCHAR2,
    errormsg      OUT VARCHAR2,
    o_dailyepunch OUT SYS_REFCURSOR
)
AS
    v_cnt_e NUMBER;
    v_cnt_s NUMBER;
BEGIN
    -- Check if employee and school exists
    SELECT COUNT(*) INTO v_cnt_e FROM TBL_USERS WHERE EMPID = p_empId;
    SELECT COUNT(*) INTO v_cnt_s FROM SCHOOL WHERE SCHOOL_CODE = p_schoolId;

    IF p_flag = 'C' THEN
        IF v_cnt_e > 0 AND v_cnt_s >0 THEN
            INSERT INTO G_DAILY_EPUNCH(
                EMPID, LATTITUDE, LONGITUDE, EPHOTO, PUNCHDATETIME, KM, ADDRESS, LOCATION, SCHOOLID, ISADDRESSMATCHED
            ) 
            VALUES (
                p_empId, p_lattitude, p_longitude, p_ephoto, SYSDATE, p_km, p_address, p_location, p_schoolID, p_isaddressmatched
            );

            ret := '1';
            errormsg := 'EPunch Record inserted successfully.';
        ELSE
           ret := '-1';
           errormsg := 'Employe Or School not found.';
        END IF;
            
        -- GET TODAY'S PUNCHES
    ELSIF p_flag = 'G' THEN
        OPEN o_dailyepunch FOR
            SELECT EMPID, LATTITUDE, LONGITUDE, EPHOTO, PUNCHDATETIME, KM, ADDRESS, LOCATION
            FROM G_DAILY_EPUNCH
            WHERE EMPID = p_empId
            AND TRUNC(PUNCHDATETIME) = TRUNC(SYSDATE)
            ORDER BY PUNCHDATETIME;

        ret := '1';
        errormsg := 'Punch records retrieved successfully.';
            
    -- GET TOTAL KM IN DATE RANGE
    ELSIF p_flag = 'F' THEN
            
       OPEN o_dailyepunch FOR
        SELECT p_empId AS EMPID,
               COALESCE((
                   SELECT SUM(KM)
                   FROM G_DAILY_EPUNCH
                   WHERE EMPID = p_empId
                     AND TRUNC(PUNCHDATETIME) BETWEEN TRUNC(p_fromDate) AND TRUNC(p_toDate)
               ), 0) AS TOTAL_KM
        FROM DUAL;

            ret := '1';
            errormsg := 'Total KM calculated successfully.';
    
    -- GET ASSIGNED SCHOOLS OF THE EMP
    ELSIF p_flag = 'S' THEN
        OPEN o_dailyepunch FOR
            SELECT SCHOOL_CODE AS SCODE, SCHOOL_NAME AS SNAME, (SADDRESS || ', ' || CITY || ', ' || STATE) AS ADDRESS FROM SCHOOL WHERE EMPID = p_empId;
        ret := '1';
        errormsg := 'success';
    ELSE
        ret := '-1';
        errormsg := 'Invalid flag.';
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        ret := '-1';
        errormsg := 'Unexpected error: ' || SQLERRM;
END;
/




-- For testing purpose: dummy users


INSERT INTO g_USERS(CUSTOMERiD, FIRSTNAME, USERNAME, PASSWORDHASH, ISACTIVE, CREATEDDATE)
VALUES ('LDPLUP071', 'Harshit', 'LDPLUP071', 'uRMpdr6YHCKSlWS64UdGKg==', 1, sysdate);

INSERT INTO g_USERS(CUSTOMERiD, FIRSTNAME, USERNAME, PASSWORDHASH, ISACTIVE, CREATEDDATE)
VALUES ('LDPLUP070', 'Ankit', 'LDPLUP070', 'uRMpdr6YHCKSlWS64UdGKg==', 1, sysdate);

COMMIT;

INSERT INTO G_USERROLES(USERID, ROLEID, ASSIGNEDDATE)
VALUES (1,1,SYSDATE);

INSERT INTO G_USERROLES(USERID, ROLEID, ASSIGNEDDATE)
VALUES (2,2, SYSDATE);

COMMIT;
