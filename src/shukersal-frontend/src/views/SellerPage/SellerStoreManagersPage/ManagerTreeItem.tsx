import { TreeItem } from "@mui/lab";
import React, { useCallback } from "react";
import { PermissionType, StoreManager } from "../../../types/appTypes";
import { Box, Button, IconButton, Stack, Typography } from "@mui/material";
import { FlexSpacer } from "../../../components/FlexSpacer";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";

import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { EditPermissionsDialog } from "./EditPermissionsDialog";
import NiceModal from "@ebay/nice-modal-react";
import { managerHasPermission } from "./util";

interface ManagerTreeItemProps {
  loggedManagerId: number;
  nodeId: string;
  manager: StoreManager;
  children?: React.ReactNode;
}

export const ManagerTreeItem = ({
  loggedManagerId,
  nodeId,
  manager,
  children,
}: ManagerTreeItemProps) => {
  const handleEdit = useCallback(() => {
    NiceModal.show(EditPermissionsDialog, { manager: manager }).then(() => {
      window.location.reload(); // getto af
    });
  }, [manager]);

  const handleDelete = useCallback(() => {
    console.log("delete");
  }, [manager]);

  return (
    <TreeItem
      nodeId={nodeId}
      label={
        <Stack
          direction="row"
          spacing={1}
          sx={{
            display: "flex",
            height: 20,
            my: 1,
          }}
        >
          {manager.storePermissions.some(
            (p) => p.permissionType === PermissionType.IsOwner
          ) && <AdminPanelSettingsIcon color="error" aria-label="Owner" />}
          <Typography>{manager.username}</Typography>
          <FlexSpacer />
          {!managerHasPermission(manager, PermissionType.IsOwner) && (
            <IconButton onClick={handleEdit}>
              <EditIcon />
            </IconButton>
          )}
          {/* Allow only the direct parent to delete */}
          {loggedManagerId === manager.parentManagerId && (
            <IconButton onClick={handleDelete}>
              <DeleteIcon />
            </IconButton>
          )}
        </Stack>
      }
    >
      {children}
    </TreeItem>
  );
};
