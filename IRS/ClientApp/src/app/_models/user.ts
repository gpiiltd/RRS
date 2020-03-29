import { KeyValuePair } from './keyValuePair';

export interface User {
    id: string;
    firstName: string;
    middleName: string;
    lastName: string;
    email1: string;
    email2: string;
    phone1: string;
    phone2: string;
    age: number;
    gender: number;
    dateOfBirth: string;
    introduction: string;
    knownAs: string;
    lastActive: string;
    areaOfOriginId: string;

    cityOfOriginId: string;

    stateOfOriginId: string;

    countryOfOriginId: string;

    photoUrl: string;
    assignedIncidences: KeyValuePair[];
    allocatedIncidences: KeyValuePair[];
    jobTitle: string;
    staffNo: string;

    departmentId: string;

    organizationId: string;

    preferredContactMethod: number;
    areaOfDeploymentId: string;

    cityOfDeploymentId: string;

    stateOfDeploymentId: string;

    countryOfDeploymentId: string;

    dateOfDeployment: string;
    dateOfSignOff: string;
    userName: string;
    password: string;
    isActive: boolean;
    mobileAppLoginPattern: string;
}
