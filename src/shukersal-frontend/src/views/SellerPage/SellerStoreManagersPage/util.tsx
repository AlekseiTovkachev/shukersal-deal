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
  };
  if (manager.childManagers.length === 0) {
    return <ManagerTreeItem key={treeItemProps.nodeId} {...treeItemProps} />;
  }
  return (
    <ManagerTreeItem key={treeItemProps.nodeId} {...treeItemProps}>
      {manager.childManagers.map((m) => makeTree(loggedManagerId, m))}
    </ManagerTreeItem>
  );
};

export const getManagerMemberIds = (rootManager: StoreManager): Set<number> => {
  const memberIds: Set<number> = new Set();

  const traverseManager = (manager: StoreManager) => {
    memberIds.add(manager.memberId);

    for (const childManager of manager.childManagers) {
      traverseManager(childManager);
    }
  };

  traverseManager(rootManager);
  return memberIds;
};


export const managerHasPermission = (manager: StoreManager, permissionType: PermissionType) => {
  return manager.storePermissions.some(p => p.permissionType === permissionType)
}