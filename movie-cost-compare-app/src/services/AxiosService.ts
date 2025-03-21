import axios, { AxiosInstance } from "axios";
import axiosRetry, {AxiosRetry} from "axios-retry";

let axiosInstance: AxiosInstance;

const axiosBaseConfig = {
  baseURL: process.env.REACT_APP_BASE_URL,
  timeout: 1000 * 20,
  withCredentials: false,
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json"
  }
};

export const getAxios = (retries:number=5, image:boolean=false) => {
  if (axiosInstance) {
    return axiosInstance;
  }
  let conf = axiosBaseConfig;
  if (image) {
    conf.headers = {
      Accept: "application/json",
      "Content-Type": "image/jpeg"
    }
  }
  axiosInstance = axios.create(conf);

  axiosRetry(axiosInstance, {retries: retries, retryDelay: axiosRetry.exponentialDelay});
  return axiosInstance;
};