export type ApiError = string;


export type ApiListData<T> = T[];
// Not supporting pagination for now
// export interface ApiListData<T> {
//     count: number,
//     next: number | null,
//     previous: number | null,
//     results: T[]
// }

export type ValidationError = {
    errors: {
        [key: string]: string[]
    }
}

export type ApiResponseListData<T> = ApiListData<T> | ApiError;

export type ApiResponse<T> = T | ApiError;  