// Sidebar route metadata
export interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    badge: string;
    badgeClass: string;
    isProtectedLink: boolean;
    isMemberLink: boolean;
    isOrganizationProtectedLink: boolean;
    isExternalLink: boolean;
    isGeneralHeaderLink: boolean;
    submenu: RouteInfo[];
}
