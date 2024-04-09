ALTER TABLE "ProtocolMapping"
ADD ("Publish_Status" int);

UPDATE "ProtocolMapping" SET "Publish_Status" = 0 WHERE "Is_Published" = 'N';
UPDATE "ProtocolMapping" SET "Publish_Status" = 1 WHERE "Is_Published" = 'Y';

ALTER TABLE "ProtocolMapping"
DROP COLUMN "Is_Published";