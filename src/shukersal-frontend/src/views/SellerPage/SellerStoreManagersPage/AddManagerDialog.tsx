import { useCallback, useEffect, useState } from "react";

import NiceModal, {
  muiDialog,
  muiDialogV5,
  useModal,
} from "@ebay/nice-modal-react";

import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControl,
  Grid,
  MenuItem,
  Select,
  TextField,
  Typography,
} from "@mui/material";
import { useSeller } from "../../../hooks/data/useSeller";
import { useNavigate } from "react-router-dom";
import {
  ProductPostFormFields,
  StorePostFormFields,
} from "../../../types/formTypes";
import { FormProvider, useForm } from "react-hook-form";
import { FlexSpacer } from "../../../components/FlexSpacer";
import { LoadingButton } from "@mui/lab";
import { errorToString } from "../../../util";
import { useSellerStore } from "../../../hooks/data/useSellerStore";
import { useMembers } from "../../../hooks/data/useMembers";
import useMemo from "react";
import { useManagers } from "./useManagers";
import { getManagerMemberIds } from "./util";

interface AddManagerDialogProps {
  storeId: number;
  bossId: number;
  asOwner: boolean;
}

export const AddManagerDialog = NiceModal.create(
  ({ storeId, bossId, asOwner }: AddManagerDialogProps) => {
    const modal = useModal();
    const sellerStoreData = useSellerStore(storeId);
    const managersData = useManagers(storeId);

    const membersData = useMembers();

    const managerMemberIds = managersData.rootManager
      ? getManagerMemberIds(managersData.rootManager)
      : new Set();

    // Members without already existing managers
    const filteredMembers = membersData.members.filter(
      (m) => !managerMemberIds.has(m.id)
    );

    const [selectedMemberId, setSelectedMemberId] = useState<number | null>(
      null
    );

    const handleClose = useCallback(() => {
      modal.hide();
    }, [modal]);

    const handleSubmit = useCallback(async () => {
      if (!selectedMemberId) return;

      await sellerStoreData.createManager({
        memberId: selectedMemberId,
        bossId: bossId,
        storeId: storeId,
        owner: asOwner,
      });
      modal.resolve();
      handleClose();
    }, [storeId, bossId, asOwner, sellerStoreData, handleClose]);

    useEffect(() => {
      managersData.getStoreManagers();
      membersData.getMembers();
    }, []);

    return (
      <Dialog {...muiDialogV5(modal)}>
        <DialogTitle>Add {asOwner ? "Owner" : "Manager"}</DialogTitle>
        <DialogContent>
          <Grid container rowSpacing={2} width="100%">
            <Grid xs={12} item>
              <Typography variant="body1">Select Member:</Typography>
            </Grid>
            <Grid xs={12} item>
              <Select
                fullWidth
                labelId="member-select-label"
                id="member-select"
                value={selectedMemberId ?? ""}
                onChange={(e) => {
                  setSelectedMemberId(Number(e.target.value));
                }}
              >
                {filteredMembers.map((m) => (
                  <MenuItem key={m.id} value={m.id}>
                    {m.username}
                  </MenuItem>
                ))}
              </Select>
            </Grid>
          </Grid>
          {sellerStoreData.error && (
            <Typography variant="body1" color="error">
              {errorToString(sellerStoreData.error.message)}
            </Typography>
          )}
          <FlexSpacer minHeight={20} />

          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <LoadingButton
              variant="contained"
              color="secondary"
              loading={sellerStoreData.isLoadingManagerOp}
              type="submit"
              onClick={handleSubmit}
              disabled={!selectedMemberId}
            >
              Add {asOwner ? "Owner" : "Manager"}
            </LoadingButton>
          </DialogActions>
        </DialogContent>
      </Dialog>
    );
  }
);
