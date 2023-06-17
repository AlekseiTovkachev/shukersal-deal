import { useCallback, useEffect, useState } from "react";
import { Member } from "../../types/appTypes";
import { membersApi } from "../../api/membersApi";
import { ApiError, ApiListData } from "../../types/apiTypes";

export const useMembers = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [members, setMembers] = useState<Member[]>([]);
  const [error, setError] = useState<string | null>(null);

  const getMembers = useCallback(async () => {
    setIsLoading(true);
    membersApi
      .getAll()
      .then((res) => {
        const _members = res as ApiListData<Member>;
        setIsLoading(false);
        setError(null);
        setMembers(_members);
      })
      .catch((res) => {
        const error = res as ApiError;
        setIsLoading(false);
        setError(error.message ?? "Error.");
        setMembers([]);
        console.error("Error while fetching members: ", error);
      });
  }, []);

  return {
    getMembers: getMembers,
    isLoading: isLoading,
    members: members,
    error: error,
  };
};
