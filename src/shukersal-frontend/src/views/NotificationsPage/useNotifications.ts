import { useCallback, useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { notificationsApi } from "../../api/notificationsApi";
import { ApiError, ApiListData } from "../../types/apiTypes";
import { Notification } from "../../types/appTypes";
import { useAppSelector } from "../../hooks/useAppSelector";

export const useNotifications = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [nts, setNts] = useState<Notification[]>([]);
  const [error, setError] = useState<string | null>(null);
  const authData = useAuth();
  const memberId = authData.currentMemberData?.currentMember.id;

  const notificationTrigger = useAppSelector(
    (state) => state.cart.notificationTrigger
  );

  const getNotifications = useCallback(async () => {
    if (memberId)
      notificationsApi
        .getAll(memberId)
        .then((res) => {
          const _notifications = res as ApiListData<Notification>;
          setIsLoading(false);
          setError(null);
          setNts(_notifications);
        })
        .catch((res) => {
          const error = res as ApiError;
          setIsLoading(false);
          setError(error.message ?? "Error.");
          setNts([]);
          console.error("Error while fetching notifications: ", error);
        });
  }, [memberId]);

  const deleteNotification = useCallback(
    async (notificationId: number) => {
      notificationsApi
        .delete(notificationId)
        .then((res) => {
          setIsLoading(false);
          setError(null);
          setNts(nts.filter((n) => n.id !== notificationId));
        })
        .catch((res) => {
          const error = res as ApiError;
          setIsLoading(false);
          setError(error.message ?? "Error.");
          console.error("Error while deleting notification: ", error);
        });
    },
    [setNts, nts]
  );
  const deleteAllNotifications = useCallback(async () => {
    if (memberId)
      notificationsApi
        .deleteAll(memberId)
        .then((res) => {
          setIsLoading(false);
          setError(null);
          setNts([]);
        })
        .catch((res) => {
          const error = res as ApiError;
          setIsLoading(false);
          setError(error.message ?? "Error.");
          console.error("Error while deleting all notifications: ", error);
        });
  }, [memberId, setNts]);

  useEffect(() => {
    getNotifications();
  }, [notificationTrigger]);

  return {
    isLoading: isLoading,
    error: error,
    nts: nts,

    getNotifications: getNotifications,
    deleteNotification: deleteNotification,
    deleteAllNotifications: deleteAllNotifications,
  };
};
