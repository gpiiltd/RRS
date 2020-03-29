import { MonthlyDataForDashboard } from './monthlyDataForDashboard';

export interface HazardDashboard {
    // organizationId: string,
    totalItems: number,


    // region yearly data
    openItems: number,
    closedItems: number,
    underReviewItems: number,
    reopenedItems: number,
    newItems: number,
    resolvedItems: number,

    // regions All  time data
    allUnderReviewItems: number,
    allClosedItems: number,
    allOpenItems: number,
    allReopenedItems: number,
    items: MonthlyDataForDashboard[]
}
