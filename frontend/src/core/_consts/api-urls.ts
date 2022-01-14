export const USER = {
    GET_BY_ID: "/User/GetUserById",
    GET_USERS: "/User/GetUsers",
    CREATE: "/User/UserRegister",
    LOGIN:"/User/Login",
    CONFIRM_ACCOUNT:"/User/SetUserStatus",
    DELETE: (userId:number) => `api/User/DeleteUser?userId=${userId}`
    //UPDATE
};

export const GAME = {
    GET_GAMES: "/Game/GetGames",
    CREATE: "/Game/CreateGame",
};