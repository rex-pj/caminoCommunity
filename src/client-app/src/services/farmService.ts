import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const FarmService = class extends BaseService {
  create = async (request) => {
    return axios.post(apiConfig.paths.farms.post.postFarm, request, {
      headers: {
        "Content-Type": "multipart/form-data",
        "X-FileUpload-Skip-ContentType": "",
      },
    });
  };

  update = async (request) => {
    const id = request.get("id");
    return axios.put(`${apiConfig.paths.farms.put.putFarm}/${id}`, request, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  };

  delete = async (id) => {
    return axios.delete(`${apiConfig.paths.farms.delete.deleteFarm}/${id}`);
  };
};

export default FarmService;
