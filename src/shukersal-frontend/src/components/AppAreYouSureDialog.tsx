import { useCallback, useState } from "react";

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
  Typography,
} from "@mui/material";
import { FlexSpacer } from "./FlexSpacer";
import { LoadingButton } from "@mui/lab";

interface AppAreYouSureDialogProps {
  onYes?: () => void | Promise<void>;
  onNo?: () => void | Promise<void>;
  bodyText?: string;
}

export const AppAreYouSureDialog = NiceModal.create(
  ({ onYes, onNo, bodyText }: AppAreYouSureDialogProps) => {
    const modal = useModal();

    const [isLoading, setIsLoading] = useState(false);

    const handleNo = useCallback(async () => {
      setIsLoading(true);
      await onNo?.();
      setIsLoading(false);
      modal.hide();
    }, [modal, onNo, setIsLoading]);

    const handleYes = useCallback(async () => {
      setIsLoading(true);
      await onYes?.();
      setIsLoading(false);
      modal.hide();
    }, [modal, onYes, setIsLoading]);

    return (
      <Dialog {...muiDialogV5(modal)}>
        <DialogTitle>Are You Sure?</DialogTitle>
        <DialogContent>
          {bodyText && (
            <>
              <DialogContentText>{bodyText}</DialogContentText>
              <FlexSpacer minHeight={100} />
            </>
          )}
          <DialogActions>
            <Button onClick={handleNo}>Cancel</Button>
            <LoadingButton
              variant="contained"
              color="secondary"
              loading={isLoading}
              onClick={handleYes}
            >
              Yes
            </LoadingButton>
          </DialogActions>
        </DialogContent>
      </Dialog>
    );
  }
);
