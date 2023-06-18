import { useCallback, useEffect, useState } from "react";

import { Box, Button, IconButton, Snackbar, Typography } from "@mui/material";

import { NTStart, NTStop } from "../../notifications/notificationsManager";
import {
  Notification,
  NotificationDisplay,
  ntTypeData,
} from "../../types/appTypes";
import dayjs from "dayjs";
import { API_DATE_TIME_FORMAT } from "../../_configuration";
import { useAuth } from "../../hooks/useAuth";
import React from "react";

import CloseIcon from "@mui/icons-material/Close";
import { useAppDispatch } from "../../hooks/useAppDispatch";
import { updateNotification } from "../../redux/cartSlice";

const errorNotification: Notification = {
  id: 0,
  createdAt: dayjs().format(API_DATE_TIME_FORMAT),
  memberId: 0,
  message: "Error parsing notification",
  notificationType: 1,
};

function fixNT(obj: any): any {
  const fixedObj: any = {};
  for (const key in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, key)) {
      const fixedKey = key.charAt(0).toLowerCase() + key.slice(1);
      fixedObj[fixedKey] = obj[key];
    }
  }
  return fixedObj;
}

const parseNotification = (data: string): Notification => {
  try {
    return fixNT(JSON.parse(data)) as unknown as Notification;
  } catch (e) {
    console.log("error: ", e);
    return errorNotification;
  }
};

export const NotificationSnackbar = () => {
  const dispatch = useAppDispatch();
  const [lastNt, setLastNt] = useState<Notification>();
  const [ntDisplay, setNtDisplay] = useState<NotificationDisplay>(
    ntTypeData[1]
  );
  const [open, setOpen] = useState(false);

  const authData = useAuth();

  const onNotification = useCallback(
    (data: string) => {
      setOpen(true);
      const nt = parseNotification(data);
      dispatch(updateNotification(nt));
      setLastNt(nt);
      setNtDisplay(ntTypeData[nt.notificationType]);
    },
    [setLastNt, setNtDisplay]
  );

  const handleClose = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setOpen(false);
  };

  const action = (
    <React.Fragment>
      <IconButton
        size="small"
        aria-label="close"
        color="inherit"
        onClick={handleClose}
      >
        <CloseIcon fontSize="small" />
      </IconButton>
    </React.Fragment>
  );

  useEffect(() => {
    if (authData.isLoggedIn) {
      NTStart(
        authData.currentMemberData?.currentMember.id ?? 0,
        onNotification
      );
    }
    return () => {
      NTStop();
    };
  }, []);

  return (
    <>
      <Snackbar
        open={open}
        autoHideDuration={6000}
        onClose={(event, reason) => {
          handleClose(event);
        }}
        action={action}
        message={
          <Box sx={{ display: "flex" }}>
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
                mr: 2,
              }}
            >
              {ntDisplay.icon}
            </Box>

            <Box>
              <Typography variant="body1">{ntDisplay.title}</Typography>
              <Typography variant="caption">{lastNt?.message}</Typography>
            </Box>
          </Box>
        }
      />
    </>
  );
};
