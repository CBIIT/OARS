ALTER TABLE "ProtocolPhase" 
ADD ("Is_Enabled" char(1));

ALTER TABLE "Profile_Field" 
ADD ("Update_Date" timestamp);

ALTER TABLE "Profile_DataCategory" 
ADD ("Update_Date" timestamp);

ALTER TABLE "ProtocolMapping" 
ADD ("Update_Date" timestamp);

ALTER TABLE "ProtocolCategoryStatus" 
ADD ("Update_Date" timestamp);

ALTER TABLE "ProtocolDictionaryMapping"  
ADD ("Update_Date" timestamp);
