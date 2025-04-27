import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import '../data/api/article/article_api_service.dart';

class ApiModule {
  static void init(GetIt getIt) {
    getIt.registerLazySingleton(() => Dio());
    getIt.registerLazySingleton(() => ArticleApiService(getIt()));
  }
}