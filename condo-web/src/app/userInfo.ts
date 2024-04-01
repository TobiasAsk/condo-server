import { Condominium } from "./condominium";

export interface UserInfo {
  userName: string;
  picture: string;
  condominium: Condominium;
  id: string;
}

export interface MeResponse {
  clientPrincipal: UserInfo
}
