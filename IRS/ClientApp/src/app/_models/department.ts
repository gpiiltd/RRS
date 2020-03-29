import { KeyValuePair } from 'app/_models/keyValuePair';
export interface Department {
    id: string,
    deptCode: string,
    deptName: string,
    description: string,
    organizationId: string,
    organization: KeyValuePair

}
