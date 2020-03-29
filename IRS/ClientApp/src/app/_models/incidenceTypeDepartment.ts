import { KeyValuePair } from './keyValuePair';

export interface IncidenceTypeDepartment {
    id: string;
    //serialNumber: number;
    incidenceTypeId: string;
    incidenceType: KeyValuePair[];
    departmentId: string;
    department: KeyValuePair[];
    organizationId: string;
    organization: KeyValuePair[];
}
