import { Condominium } from "./condominium";

export interface UserInfo {
  userName: string;
  picture: string;
  condominium: Condominium;
}

export interface MeResponse {
  clientPrincipal: UserInfo
}
