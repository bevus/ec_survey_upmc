Installation :
-	Ouvrir le projet dans Visual Studio en allant dans file->open->Project/Solution.
-	Une fois le projet chargé, lancez la compilation en cliquant sur Build->Build Solution.
-	Une fois la compilation terminée, lancez le fichier Survey/Main.aspx.
-	Une page web s’ouvre dans le navigateur, ou sont listé les polls présent dans la base de données.
Pour tester le fonctionnement du module suivez les étapes suivantes :
-	Insérez le survey dans la base de données : exemple d’insertion dans le script test_survey_insert.sql.
-	Insérez les ateliers session, workshop et meeting : exemple dans le script atelier_meeting_test.sql.
-	Vérifiez la cohérence du modèle.
-	Lancez la procédure stocké SEL_generate_survey pour créer les tables où seront stockées les réponses des utilisateurs.
-	Allez sur la page Main.aspx, le nouveau poll inséré devrais s’afficher dans la liste des polls.
-	Cliquez sur ce poll, dans la fenêtre de configuration qui apparait configurez les paramètres de génération puis lancez la génération en cliquant sur le bouton générer.
-	Une page s’affiche avec deux liens, un pour voir le dashboard généré un autre lien pour voir le formulaire de survey.
-	Sur le dashboard vous pouvez extraire les données en cliquant sur les boutons en haut de la page.
