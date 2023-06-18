import { useCallback, useEffect, useState } from "react";

import NiceModal, {
  muiDialog,
  muiDialogV5,
  useModal,
} from "@ebay/nice-modal-react";

import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Divider,
  FormControl,
  Grid,
  MenuItem,
  Select,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import { FlexSpacer } from "../../../components/FlexSpacer";
import { PermissionType, StoreManager } from "../../../types/appTypes";
import { storeManagersApi } from "../../../api/storesApi";
import React from "react";

const PermissionEntries = ({
  handleChange,
  manager,
}: {
  handleChange: (permissionType: number, value: boolean) => void;
  manager: StoreManager;
}) => {
  const perms = {
    [PermissionType.ManageProducts]: "Manage Products",
    [PermissionType.ManageDiscounts]: "Manage Discounts",
    [PermissionType.AppointOwner]: "Appoint Owner",
    [PermissionType.RemoveOwner]: "Remove Owner",
    [PermissionType.AppointManager]: "Appoint Manager",
    [PermissionType.EditManagerPermissions]: "Edit Manager Permissions",
    [PermissionType.RemoveManager]: "Remove Manager",
    [PermissionType.GetManagerInfo]: "Get Manager Info",
    // [PermissionType.GetHistoryPermission]: "Get History Permission",
  };
  return (
    <>
      {Object.keys(perms).map((k, idx) => (
        <React.Fragment key={idx}>
          {idx > 0 && (
            <Grid xs={12} item>
              <Divider orientation="horizontal" />
            </Grid>
          )}
          <Grid xs={12} item>
            <Box sx={{ display: "flex", width: "100%" }}>
              <Typography variant="h5">
                {
                  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
                  //@ts-expect-error
                  perms[Number(k)] ?? ""
                }
              </Typography>
              <FlexSpacer />
              <Switch
                defaultChecked={manager.storePermissions.some(
                  (p) => p.permissionType === Number(k)
                )}
                onChange={(e) => {
                  handleChange(Number(k), e.target.checked);
                }}
              />
            </Box>
          </Grid>
        </React.Fragment>
      ))}
    </>
  );
};

interface EditPermissionsDialogProps {
  manager: StoreManager;
}

export const EditPermissionsDialog = NiceModal.create(
  ({ manager }: EditPermissionsDialogProps) => {
    const modal = useModal();

    const handleChange = useCallback(
      (permissionType: number, value: boolean) => {
        if (value) {
          storeManagersApi.addPermission(manager.id, permissionType);
        } else {
          storeManagersApi.removePermission(manager.id, permissionType);
        }
      },
      [manager]
    );

    const handleClose = useCallback(() => {
      modal.hide();
    }, [modal]);

    return (
      <Dialog {...muiDialogV5(modal)}>
        <DialogTitle>Manager Permissions for {manager.username}</DialogTitle>
        <DialogContent>
          <Grid container rowSpacing={2} width="100%">
            <Grid xs={12} item>
              <PermissionEntries
                handleChange={handleChange}
                manager={manager}
              />
            </Grid>
          </Grid>
          <FlexSpacer minHeight={20} />

          <DialogActions>
            <Button onClick={handleClose}>Done</Button>
          </DialogActions>
        </DialogContent>
      </Dialog>
    );
  }
);
