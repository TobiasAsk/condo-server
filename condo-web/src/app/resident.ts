import { Condominium } from "./condominium";

export interface Resident {
    id: string;
    name: string;
    picture: string;
    condominium: Condominium;
}

export interface ResidentResponse {
    data: { resident_by_pk: Resident }
}