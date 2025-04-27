import 'package:get_it/get_it.dart';
import '../data/repository/articles_repository_impl.dart';
import '../domain/abstractions/articles_repository.dart';
import '../domain/usecase/article/get_article_use_case.dart';
import '../domain/usecase/article/get_articles_use_case.dart';

class RepositoryModule {
  static void init(GetIt getIt) {
    getIt.registerLazySingleton<ArticlesRepository>(() => ArticlesRepositoryImpl(getIt()));
    getIt.registerLazySingleton(() => GetArticleUseCase(getIt()));
    getIt.registerLazySingleton(() => GetArticlesUseCase(getIt()));
  }
}