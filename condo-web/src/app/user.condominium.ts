import { InjectionToken, inject } from "@angular/core";
import { ResidentService } from "./resident.service";

export const CONDOMINIUM_ID = new InjectionToken<string>('user.condominium.id', {
    factory: () => inject(ResidentService).resident.condominium.id
});
