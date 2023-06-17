import {
  PermissionType,
  StoreManager,
  StorePermission,
} from "../../../types/appTypes";
import { ManagerTreeItem } from "./ManagerTreeItem";

export const makeTree = (loggedManagerId: number, manager: StoreManager) => {
  const treeItemProps = {
    loggedManagerId: loggedManagerId,
    nodeId: manager.id.toString(),
    manager: manager,
    managerMemberName: manager.memberId.toString(), // TODO: Connect name when BE implements
  };
  if (manager.childManagers.length === 0) {
    return <ManagerTreeItem {...treeItemProps} />;
  }
  return (
    <ManagerTreeItem {...treeItemProps}>
      {manager.childManagers.map((m) => makeTree(loggedManagerId, m))}
    </ManagerTreeItem>
  );
};
