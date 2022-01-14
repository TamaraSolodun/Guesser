import { userReducer } from "./user/reducer";
import { combineReducers } from "redux"
import { applyMiddleware, compose, createStore } from "@reduxjs/toolkit";
import thunk from "redux-thunk";
import settingsReducer from "./settings/reducer";
import {createLogger} from "redux-logger"
import gameReducer from "./games/reducer";

const rootReducer = combineReducers({userRedux: userReducer, settingsRedux: settingsReducer, gameRedux: gameReducer});

const composeEnhancers = compose;
const logger = createLogger({});
const store = createStore(rootReducer, composeEnhancers(applyMiddleware(thunk), applyMiddleware(logger)));

export type Store = ReturnType<typeof rootReducer>;

export default store;