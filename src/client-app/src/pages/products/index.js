import React, { useEffect, useState, useRef } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constants";
import { useLazyQuery, useMutation } from "@apollo/client";
import { productQueries } from "../../graphql/fetching/queries";
import { productMutations } from "../../graphql/fetching/mutations";
import { useParams } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import { authClient } from "../../graphql/client";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import InfiniteScroll from "react-infinite-scroll-component";
import { apiConfig } from "../../config/api-config";

export default (function (props) {
  const { pageNumber } = useParams();
  const [state, dispatch] = useStore(false);
  const [products, setProducts] = useState([]);
  const pageRef = useRef({ pageNumber: pageNumber ? pageNumber : 1 });
  const [fetchProducts, { loading, data, error, refetch }] = useLazyQuery(
    productQueries.GET_PRODUCTS,
    {
      onCompleted: (data) => {
        setPageInfo(data);
        onFetchCompleted(data);
      },
      fetchPolicy: "cache-and-network",
    }
  );

  const [deleteProduct] = useMutation(productMutations.DELETE_PRODUCT, {
    client: authClient,
  });

  const setPageInfo = (data) => {
    const {
      products: {
        totalPage,
        totalResult,
        filter: { page },
      },
    } = data;
    pageRef.current.totalPage = totalPage;
    pageRef.current.currentPage = page;
    pageRef.current.totalResult = totalResult;
  };

  const onFetchCompleted = (data) => {
    const {
      products: { collections },
    } = data;
    const productCollections = parseCollections(collections);
    setProducts([...products, ...productCollections]);
  };

  useEffect(() => {
    if (state.type === "PRODUCT_UPDATE" || state.type === "PRODUCT_DELETE") {
      refetch();
    }
  }, [state, refetch]);

  useEffect(() => {
    const page = pageRef.current.pageNumber;
    fetchProducts({
      variables: {
        criterias: {
          page: page ? parseInt(page) : 1,
        },
      },
    });
  }, [fetchProducts]);

  const parseCollections = (collections) => {
    return collections.map((item) => {
      let product = { ...item };
      product.url = `${UrlConstant.Product.url}${product.id}`;
      if (product.pictures && product.pictures.length > 0) {
        const picture = product.pictures[0];
        if (picture.pictureId > 0) {
          product.pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`;
        }
      }

      product.creator = {
        createdDate: item.createdDate,
        profileUrl: `/profile/${item.createdByIdentityId}`,
        name: item.createdBy,
      };

      if (item.createdByPhotoId) {
        product.creator.photoUrl = `${apiConfig.paths.userPhotos.get.getAvatar}/${item.createdByPhotoId}`;
      }

      if (product.farms) {
        product.farms = product.farms.map((pf) => {
          let productFarm = { ...pf };
          productFarm.url = `/farms/${pf.id}`;
          return productFarm;
        });
      }

      return product;
    });
  };

  const breadcrumbs = [
    {
      isActived: true,
      title: "Product",
    },
  ];

  const onOpenDeleteConfirmation = (e) => {
    const { title, innerModal, message, id } = e;
    dispatch("OPEN_MODAL", {
      data: {
        title: title,
        children: message,
        id: id,
      },
      execution: { onDelete: onDelete },
      options: {
        isOpen: true,
        innerModal: innerModal,
        position: "fixed",
      },
    });
  };

  const onDelete = (id) => {
    deleteProduct({
      variables: {
        criterias: { id },
      },
    }).then(() => {
      refetch();
    });
  };

  const fetchMoreData = () => {
    if (pageRef.current.pageNumber === pageRef.current.totalPage) {
      return;
    }
    pageRef.current.pageNumber += 1;
    fetchProducts({
      variables: {
        criterias: {
          page: pageRef.current.pageNumber,
        },
      },
    });
  };

  const checkHasData = () => {
    return data && pageRef.current.totalResult && products.length >= 0;
  };

  return (
    <DefaultLayout
      isLoading={!!loading}
      hasData={checkHasData()}
      hasError={!!error}
    >
      <Breadcrumb list={breadcrumbs} className="px-2" />
      <InfiniteScroll
        style={{ overflowX: "hidden" }}
        dataLength={pageRef.current.totalResult ?? 0}
        next={fetchMoreData}
        hasMore={pageRef.current.currentPage < pageRef.current.totalPage}
        loader={<h4>Loading...</h4>}
      >
        <Product
          onOpenDeleteConfirmation={onOpenDeleteConfirmation}
          products={products}
          baseUrl="/products"
        />
      </InfiniteScroll>
    </DefaultLayout>
  );
});
