import { Dispatch } from "@reduxjs/toolkit"
import { UPDATE_SETTINGS } from "../action-types"
import { ISettings } from "./interfaces"

export const updateSettings = (settingsToUpdate:ISettings) => async(dispatch: Dispatch): Promise<void> => {
    dispatch({type:UPDATE_SETTINGS, payload:settingsToUpdate});
}