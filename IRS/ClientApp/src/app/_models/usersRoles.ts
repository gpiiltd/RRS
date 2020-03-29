import { KeyValuePair } from './keyValuePair';

export interface UsersRoles {
    id: string;
    firstName: string;
    middleName?: string;
    lastName: string;
    userName: string;
    email1?: string;
    email2?: string;
    phone1: string;
    phone2: string;
    age: number;
    gender: number;
    dateOfBirth: string;
    introduction?: string;
    knownAs: string;
    dateCreated: string;
    lastActive: string;
    photoUrl: string;
    jobTitle: string;
    staffNo: string;

    departmentId: string;
    userDepartment: KeyValuePair;

    organizationId: string;
    userOrganization: KeyValuePair;
    roles: string[];
    availableRoles: string[];
    userRoleString: string;
    AvailableRolesString: string;
}
