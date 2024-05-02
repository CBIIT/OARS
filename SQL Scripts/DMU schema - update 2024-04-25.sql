ALTER TABLE DMU."ProtocolFormMapping"
ADD ("Protocol_Category_Id" integer)

ALTER TABLE DMU."ProtocolFormMapping"
ADD FOREIGN KEY ("Protocol_Category_Id") REFERENCES DMU."ProtocolDataCategory"("Protocol_Category_Id")