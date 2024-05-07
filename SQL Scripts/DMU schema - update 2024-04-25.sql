ALTER TABLE DMU."ProtocolFormMapping"
ADD ("Protocol_Category_Id" integer)

ALTER TABLE DMU."ProtocolFormMapping"
ADD FOREIGN KEY ("Protocol_Category_Id") REFERENCES DMU."ProtocolDataCategory"("Protocol_Category_Id")

ALTER TABLE DMU."ProtocolDataCategory"
ADD ("Is_Multi_Form" CHAR(1))