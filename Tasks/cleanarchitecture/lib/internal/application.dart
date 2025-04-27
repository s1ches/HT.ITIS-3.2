import 'package:get_it/get_it.dart';
import 'api_module.dart';
import 'repository_module.dart';

final getIt = GetIt.instance;

class Application {
  static void init() {
    ApiModule.init(getIt);
    RepositoryModule.init(getIt);
  }
}