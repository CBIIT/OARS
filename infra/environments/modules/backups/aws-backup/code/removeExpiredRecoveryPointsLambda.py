import boto3
import logging
import os

backupvault=os.environ['BACKUP_VAULT_NAME']

#setup simple logging for INFO
logger = logging.getLogger()
logger.setLevel(logging.INFO)

#define the connection
backup = boto3.client("backup")

def lambda_handler_remove_expired(event, context):
    recoverypoints = backup.list_recovery_points_by_backup_vault(BackupVaultName=backupvault)

    logger.info("Checking Recover points in vault: " + backupvault)
    for recoverypoint in recoverypoints["RecoveryPoints"]:
        if recoverypoint["Status"] == "EXPIRED":
            logger.info("Deleting expired recovery point: " + recoverypoint["RecoveryPointArn"])
            result = backup.delete_recovery_point(BackupVaultName=backupvault,RecoveryPointArn=recoverypoint["RecoveryPointArn"])