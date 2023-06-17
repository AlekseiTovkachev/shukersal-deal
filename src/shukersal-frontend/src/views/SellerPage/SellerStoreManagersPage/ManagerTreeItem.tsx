import { TreeItem } from "@mui/lab";
import React from "react";
import { PermissionType, StoreManager } from "../../../types/appTypes";
import { Box, Button, IconButton, Stack, Typography } from "@mui/material";
import { FlexSpacer } from "../../../components/FlexSpacer";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";

import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

interface ManagerTreeItemProps {
  loggedManagerId: number;
  nodeId: string;
  manager: StoreManager;
  managerMemberName: string;
  children?: React.ReactNode;
}

export const ManagerTreeItem = ({
  loggedManagerId,
  nodeId,
  manager,
  managerMemberName,
  children,
}: ManagerTreeItemProps) => {
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
          <Typography>{managerMemberName}</Typography>
          <FlexSpacer />
          <IconButton>
            <EditIcon />
          </IconButton>
          {/* Allow only the direct parent to delete */}
          {loggedManagerId === manager.parentManagerId && (
            <IconButton>
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
