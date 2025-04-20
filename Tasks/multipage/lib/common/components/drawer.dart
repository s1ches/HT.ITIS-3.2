import 'package:flutter/material.dart';
import '../../pages/main_page.dart';
import '../../pages/not_main_page.dart';

class MainDrawer extends StatelessWidget {
  const MainDrawer({super.key});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [
          const DrawerHeader(child: Text("Menu")),
          ListTile(
            title: const Text("Main Page"),
            onTap: () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => const MainPage()),
              );
            },
          ),
          ListTile(
            title: const Text("Not Main Page"),
            onTap: () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => const SecondaryPage()),
              );
            },
          ),
        ],
      ),
    );
  }
}