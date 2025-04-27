import '../../abstractions/articles_repository.dart';
import '../../model/article.dart';

class GetArticleUseCase {
  final ArticlesRepository repository;

  GetArticleUseCase(this.repository);

  Future<Article> call(int id) {
    return repository.getArticle(id);
  }
}