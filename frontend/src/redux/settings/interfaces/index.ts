import { UPDATE_SETTINGS, GET_SETTINGS } from "../../action-types";
export interface IDifficult {
    retriesCount: number;
    numberFrom: number;
    numberTo: number;
  }
export interface ISettings{
    isHintModeEnabled: boolean;
    difficult: IDifficult;
}

export interface IUpdateSettings {
    type: typeof UPDATE_SETTINGS;
    payload: ISettings;
}

export interface IGetSettings {
    type: typeof GET_SETTINGS;
    payload: ISettings;
}

export interface ISettingsReducer {
    Settings: ISettings;
}

export type SettingsActions = IGetSettings | IUpdateSettings;