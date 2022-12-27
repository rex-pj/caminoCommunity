import { apiConfig } from "../config/api-config";
import axios from "axios";
import BaseService from "./baseService";

const ProductService = class extends BaseService {
  create = async (request) => {
    return axios.post(apiConfig.paths.products.post.postProduct, request, {
      headers: {
        "Content-Type": "multipart/form-data",
        "X-FileUpload-Skip-ContentType": "",
      },
    });
  };

  update = async (request) => {
    return axios.put(
      `${apiConfig.paths.products.put.putProduct}/${request.id}`,
      request,
      {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }
    );
  };

  delete = async (id) => {
    return axios.delete(
      `${apiConfig.paths.products.delete.deleteProduct}/${id}`
    );
  };
};

export default ProductService;
