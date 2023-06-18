import { useCallback } from "react";

import NiceModal, { muiDialogV5, useModal } from "@ebay/nice-modal-react";

import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { FlexSpacer } from "./FlexSpacer";

interface AppAreYouSureDialogProps {
  title: string;
  onClose?: () => void | Promise<void>;
  body?: React.ReactNode;
}

export const SuccessDialog = NiceModal.create(
  ({ title, onClose: onClose, body }: AppAreYouSureDialogProps) => {
    const modal = useModal();

    const handleClose = useCallback(async () => {
      await onClose?.();
      modal.hide();
    }, [modal, onClose]);

    return (
      <Dialog {...muiDialogV5(modal)}>
        <DialogTitle>{title}</DialogTitle>
        <DialogContent>
          {body && (
            <>
              {body}
              <FlexSpacer minHeight={50} />
            </>
          )}
          <DialogActions>
            <Button onClick={handleClose} variant="contained">Close</Button>
          </DialogActions>
        </DialogContent>
      </Dialog>
    );
  }
);
